namespace InventoryControl.WebApi.Models
{
    public class ProductModel
    {      
        public int Id { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
