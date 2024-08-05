using InventoryControl.Application.Queries.Products;
using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Repositories;
using InventoryControl.Infra.Data.Abstractions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InventoryUnitTests.Handlers
{
    public class GetProductByIdHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly GetProductByIdHandler _handler;

        public GetProductByIdHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _handler = new GetProductByIdHandler(_unitOfWorkMock.Object);

            _unitOfWorkMock.SetupGet(u => u.Products).Returns(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var query = new GetProductByIdQuery { Id = 1 };
            var product = new Product { Id = 1, PartNumber = "123", Name = "Product 1" };

            _productRepositoryMock.Setup(r => r.GetById(query.Id))
                .ReturnsAsync(product);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.PartNumber, result.PartNumber);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var query = new GetProductByIdQuery { Id = 1 };

            _productRepositoryMock.Setup(r => r.GetById(query.Id))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
