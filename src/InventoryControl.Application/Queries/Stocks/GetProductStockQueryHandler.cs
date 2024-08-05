using InventoryControl.Application.Notifications;
using InventoryControl.Infra.Data.Abstractions;
using MediatR;

namespace InventoryControl.Application.Queries.Stocks
{
    public class GetProductStockQueryHandler : IRequestHandler<GetProductStockQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public GetProductStockQueryHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<int> Handle(GetProductStockQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetById(request.ProductId);
            if (product == null)
            {
                await _mediator.Publish(new NotificationCommand("ProductNotFound", $"Product with ID {request.ProductId} does not exist."), cancellationToken);
                return -1;
            }

            var stock = (await _unitOfWork.Stocks.Query(s => s.ProductId == request.ProductId)).FirstOrDefault();
            return stock?.Quantity ?? 0;
        }
    }
}
