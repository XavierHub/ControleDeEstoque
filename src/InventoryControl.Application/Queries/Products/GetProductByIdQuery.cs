using InventoryControl.Domain;
using MediatR;

namespace InventoryControl.Application.Queries.Products
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }
}
