using MediatR;

namespace InventoryControl.Application.Queries.Stocks
{
    public class GetProductStockQuery : IRequest<int>
    {
        public int ProductId { get; set; }

        public GetProductStockQuery(int productId)
        {
            ProductId = productId;
        }
    }
}
