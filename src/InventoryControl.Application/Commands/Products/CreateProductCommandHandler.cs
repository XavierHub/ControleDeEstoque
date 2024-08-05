using InventoryControl.Application.Notifications;
using InventoryControl.Domain;
using InventoryControl.Infra.Data.Abstractions;
using MediatR;

namespace InventoryControl.Application.Commands.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Verifica se o partNumber já existe
                var existingProduct = await _unitOfWork.Products.Query(p => p.PartNumber == request.PartNumber);
                if (existingProduct != null && existingProduct.Any())
                {
                    await _mediator.Publish(new NotificationCommand("PartNumber", $"A product with the same part number '{request.PartNumber}' already exists"), cancellationToken);
                    return default;
                }

                var product = new Product
                {
                    PartNumber = request.PartNumber,
                    Name = request.Name,                    
                };

                _unitOfWork.BeginTransaction();

                var productId = Convert.ToInt32(await _unitOfWork.Products.Insert(product));

                _unitOfWork.Commit();
                return productId;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                await _mediator.Publish(new NotificationCommand("Product", $"An error occurred while creating a product with PartNumber: {request.PartNumber}", ex), cancellationToken);
            }

            return default;
        }
    }
}
