using InventoryControl.Domain;
using InventoryControl.Infra.Data.Abstractions;
using MediatR;

namespace InventoryControl.Application.Queries.Products
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Products.GetAll();
        }
    }
}
