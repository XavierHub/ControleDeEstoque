using InventoryControl.Domain;
using InventoryControl.Infra.Data.Abstractions;
using MediatR;

namespace InventoryControl.Application.Queries.Consumptions
{
    public class GetProductDailyConsumptionQueryHandler : IRequestHandler<GetProductDailyConsumptionQuery, Consumption>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductDailyConsumptionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Consumption> Handle(GetProductDailyConsumptionQuery request, CancellationToken cancellationToken)
        {
            var consumption = await _unitOfWork.Consumptions.Get(c => c.ProductId == request.ProductId && c.ConsumptionDate == request.Date.Date);
            return consumption;
        }
    }
}
