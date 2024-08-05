using InventoryControl.Domain;
using InventoryControl.Infra.Data.Abstractions;
using MediatR;

namespace InventoryControl.Application.Queries.Products
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Products.GetById(request.Id);
        }
    }
}
