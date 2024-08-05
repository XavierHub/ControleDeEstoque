using MediatR;

namespace InventoryControl.Application.Commands.Products
{
    public abstract class ProductCommandBase
    {
        public int Id { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;        
    }

    public abstract class ProductCommand : ProductCommandBase, IRequest<int>
    {
    }

    public class CreateProductCommand : ProductCommandBase, IRequest<int>
    {
    }

    public class UpdateProductCommand : ProductCommandBase, IRequest<bool>
    {
    }

    public class DeleteProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
