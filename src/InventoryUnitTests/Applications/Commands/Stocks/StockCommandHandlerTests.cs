using InventoryControl.Application.Commands.Consumptions;
using InventoryControl.Application.Commands.Stocks;
using InventoryControl.Application.Notifications;
using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Repositories;
using InventoryControl.Infra.Data.Abstractions;
using MediatR;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InventoryUnitTests.Handlers
{
    public class StockCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<IRepository<Stock>> _stockRepositoryMock;
        private readonly Mock<IRepository<Consumption>> _consumptionRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly StockCommandHandler _handler;

        public StockCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _stockRepositoryMock = new Mock<IRepository<Stock>>();
            _consumptionRepositoryMock = new Mock<IRepository<Consumption>>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new StockCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);

            _unitOfWorkMock.SetupGet(u => u.Products).Returns(_productRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Stocks).Returns(_stockRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Consumptions).Returns(_consumptionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldPublishNotification_WhenProductNotFound()
        {
            // Arrange
            var command = new StockCommand(1, 10);
            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync((Product)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "Product" && n.Message == "Product not found"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldPublishNotification_WhenQuantityIsZero()
        {
            // Arrange
            var command = new StockCommand(1, 0);
            var product = new Product { Id = 1, PartNumber = "123", Name = "Test Product" };
            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "Stock" && n.Message == "Cannot process zero quantity."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldPublishNotification_WhenReducingStockForNonExistentProduct()
        {
            // Arrange
            var command = new StockCommand(1, -10);
            var product = new Product { Id = 1, PartNumber = "123", Name = "Test Product" };
            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _stockRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Stock, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Stock>());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "Stock" && n.Message == "Cannot reduce stock for a non-existent product."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldPublishNotification_WhenInsufficientStock()
        {
            // Arrange
            var command = new StockCommand(1, -10);
            var product = new Product { Id = 1, PartNumber = "123", Name = "Test Product" };
            var stock = new Stock { ProductId = 1, Quantity = 5 };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _stockRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Stock, bool>>>()))
                .ReturnsAsync(new[] { stock });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "Stock" && n.Message == "Insufficient stock available."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldAddStock_WhenProductExists()
        {
            // Arrange
            var command = new StockCommand(1, 10);
            var product = new Product { Id = 1, PartNumber = "123", Name = "Test Product" };
            var stock = new Stock { ProductId = 1, Quantity = 5, Total = 10, UnitPrice = 2, AveragePrice = 2 };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _stockRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Stock, bool>>>()))
                .ReturnsAsync(new[] { stock });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(u => u.BeginTransaction(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
            _stockRepositoryMock.Verify(r => r.Update(It.IsAny<Stock>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldRemoveStock_WhenStockIsSufficient()
        {
            // Arrange
            var command = new StockCommand(1, -3);
            var product = new Product { Id = 1, PartNumber = "123", Name = "Test Product" };
            var stock = new Stock { ProductId = 1, Quantity = 5, Total = 10, UnitPrice = 2, AveragePrice = 2 };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _stockRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Stock, bool>>>()))
                .ReturnsAsync(new[] { stock });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(u => u.BeginTransaction(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
            _stockRepositoryMock.Verify(r => r.Update(It.IsAny<Stock>()), Times.Once);
            _consumptionRepositoryMock.Verify(r => r.Insert(It.IsAny<Consumption>()), Times.Once);
        }        
    }
}
