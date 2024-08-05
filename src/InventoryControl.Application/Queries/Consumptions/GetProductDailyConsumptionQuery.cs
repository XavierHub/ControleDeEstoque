using InventoryControl.Domain;
using MediatR;

namespace InventoryControl.Application.Queries.Consumptions
{
    public class GetProductDailyConsumptionQuery : IRequest<Consumption>
    {
        public int ProductId { get; set; }
        public DateTime Date { get; set; }

        public GetProductDailyConsumptionQuery(int productId, DateTime date)
        {
            ProductId = productId;
            Date = date;
        }
    }
}
