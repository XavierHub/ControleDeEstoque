namespace InventoryControl.Domain
{
    public class Consumption : Entity
    {
        public int ProductId { get; set; }
        public int QuantityConsumed { get; set; }
        public DateTime ConsumptionDate { get; set; }
        public decimal TotalAveragePrice { get; set; }
        public decimal TotalCost { get; set; }

        public Product Product { get; set; }
    }
}
