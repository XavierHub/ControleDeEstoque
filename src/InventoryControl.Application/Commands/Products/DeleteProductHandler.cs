using InventoryControl.Application.Notifications;
using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Repositories;
using MediatR;

namespace InventoryControl.Application.Commands.Products
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IRepository<Product> _repository;
        private readonly IRepository<Stock> _stockRepository;
        private readonly IMediator _mediator;

        public DeleteProductHandler(IRepository<Product> repository, IRepository<Stock> stockRepository, IMediator mediator)
        {
            _repository = repository;
            _stockRepository = stockRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetById(request.Id);
            if (product == null)
            {
                await _mediator.Publish(new NotificationCommand("ProductId", $"Product id '{request.Id}' does not exist."));
                return false;
            }

            var stock = (await _stockRepository.Query(s => s.ProductId == request.Id)).FirstOrDefault();
            if (stock != null && stock.Quantity > 0)
            {
                await _mediator.Publish(new NotificationCommand("Product", $"Cannot delete product '{product.Name}' with stock remaining {stock.Quantity}."));
                return false;
            }

            return await _repository.Delete(product);
        }
    }
}
