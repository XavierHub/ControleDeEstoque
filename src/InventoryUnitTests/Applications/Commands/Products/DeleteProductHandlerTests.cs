using InventoryControl.Application.Commands.Products;
using InventoryControl.Application.Notifications;
using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Repositories;
using MediatR;
using Moq;
using System.Linq.Expressions;

namespace InventoryUnitTests.Handlers
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<IRepository<Stock>> _stockRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _stockRepositoryMock = new Mock<IRepository<Stock>>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new DeleteProductHandler(_productRepositoryMock.Object, _stockRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new DeleteProductCommand { Id = 1 };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "ProductId" && n.Message == $"Product id '{command.Id}' does not exist."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductHasStockRemaining()
        {
            // Arrange
            var command = new DeleteProductCommand { Id = 1 };
            var product = new Product { Id = 1, Name = "Test Product" };
            var stock = new Stock { ProductId = 1, Quantity = 10 };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _stockRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Stock, bool>>>()))
                .ReturnsAsync(new[] { stock });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "Product" && n.Message == $"Cannot delete product '{product.Name}' with stock remaining {stock.Quantity}."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenProductHasNoStockRemaining()
        {
            // Arrange
            var command = new DeleteProductCommand { Id = 1 };
            var product = new Product { Id = 1, Name = "Test Product" };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _stockRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Stock, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Stock>());

            _productRepositoryMock.Setup(r => r.Delete(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _productRepositoryMock.Verify(r => r.Delete(It.Is<Product>(p => p.Id == command.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenDeleteFails()
        {
            // Arrange
            var command = new DeleteProductCommand { Id = 1 };
            var product = new Product { Id = 1, Name = "Test Product" };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _stockRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Stock, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Stock>());

            _productRepositoryMock.Setup(r => r.Delete(It.IsAny<Product>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _productRepositoryMock.Verify(r => r.Delete(It.Is<Product>(p => p.Id == command.Id)), Times.Once);
        }
    }
}
