using MediatR;

namespace InventoryControl.Application.Commands.Stocks
{
    public class StockCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public StockCommand(int productId, int quantity, decimal unitPrice=0)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
