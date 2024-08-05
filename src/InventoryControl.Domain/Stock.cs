namespace InventoryControl.Domain
{
    public class Stock : Entity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Total { get; set; }
        public Product Product { get; set; }
    }
}
