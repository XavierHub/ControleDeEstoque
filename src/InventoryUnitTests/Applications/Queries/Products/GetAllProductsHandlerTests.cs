using InventoryControl.Application.Queries.Products;
using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Repositories;
using InventoryControl.Infra.Data.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InventoryUnitTests.Handlers
{
    public class GetAllProductsHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _handler = new GetAllProductsHandler(_unitOfWorkMock.Object);

            _unitOfWorkMock.SetupGet(u => u.Products).Returns(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProducts_WhenProductsExist()
        {
            // Arrange
            var query = new GetAllProductsQuery();
            var products = new List<Product>
            {
                new Product { Id = 1, PartNumber = "123", Name = "Product 1" },
                new Product { Id = 2, PartNumber = "456", Name = "Product 2" }
            };

            _productRepositoryMock.Setup(r => r.GetAll())
                .ReturnsAsync(products);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Id == 1 && p.PartNumber == "123" && p.Name == "Product 1");
            Assert.Contains(result, p => p.Id == 2 && p.PartNumber == "456" && p.Name == "Product 2");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var query = new GetAllProductsQuery();
            var products = new List<Product>();

            _productRepositoryMock.Setup(r => r.GetAll())
                .ReturnsAsync(products);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
