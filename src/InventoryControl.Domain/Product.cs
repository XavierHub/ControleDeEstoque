namespace InventoryControl.Domain
{
    public class Product : Entity
    {
        public string PartNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}

