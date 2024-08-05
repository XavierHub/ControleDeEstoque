using InventoryControl.Application.Notifications;
using InventoryControl.Infra.Data.Abstractions;
using MediatR;

namespace InventoryControl.Application.Commands.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Verifica se o produto existe
                var product = await _unitOfWork.Products.GetById(request.Id);
                if (product == null)
                {
                    await _mediator.Publish(new NotificationCommand("PartNumber", "The product to be updated does not exist"), cancellationToken);
                    return false;
                }

                // Verifica se o partNumber está sendo atualizado para um já existente
                if (product.PartNumber != request.PartNumber)
                {
                    var existingProduct = await _unitOfWork.Products.Query(p => p.PartNumber == request.PartNumber && p.Id != request.Id);
                    if (existingProduct != null && existingProduct.Any())
                    {
                        await _mediator.Publish(new NotificationCommand("PartNumber", "Another product with the same part number '{request.PartNumber}' already exists"), cancellationToken);
                        return false;
                    }
                }

                // Atualiza o produto
                product.PartNumber = request.PartNumber;
                product.Name = request.Name;                                

                _unitOfWork.BeginTransaction();

                var result = await _unitOfWork.Products.Update(product);

                _unitOfWork.Commit();
                return result;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                await _mediator.Publish(new NotificationCommand(nameof(UpdateProductCommand), "Error occurred while updating product", ex), cancellationToken);
            }
            return false;
        }
    }
}
