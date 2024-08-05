using InventoryControl.Domain;
using MediatR;

namespace InventoryControl.Application.Queries.Products
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
    }
}
