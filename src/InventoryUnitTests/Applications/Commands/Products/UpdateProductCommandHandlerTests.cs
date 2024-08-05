using InventoryControl.Application.Commands.Products;
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
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new UpdateProductCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);

            _unitOfWorkMock.SetupGet(u => u.Products).Returns(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new UpdateProductCommand { Id = 1, PartNumber = "123", Name = "Test Product" };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "PartNumber" && n.Message == "The product to be updated does not exist"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenPartNumberIsNotUnique()
        {
            // Arrange
            var command = new UpdateProductCommand { Id = 1, PartNumber = "123", Name = "Test Product" };
            var product = new Product { Id = 1, PartNumber = "456", Name = "Existing Product" };
            var existingProduct = new Product { Id = 2, PartNumber = "123", Name = "Another Product" };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(new[] { existingProduct });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "PartNumber" && n.Message == "Another product with the same part number '{request.PartNumber}' already exists"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenProductExistsAndPartNumberIsUnique()
        {
            // Arrange
            var command = new UpdateProductCommand { Id = 1, PartNumber = "123", Name = "Updated Product" };
            var product = new Product { Id = 1, PartNumber = "456", Name = "Existing Product" };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Product>());

            _productRepositoryMock.Setup(r => r.Update(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _unitOfWorkMock.Verify(u => u.BeginTransaction(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldRollbackTransaction_AndPublishNotification_WhenExceptionOccurs()
        {
            // Arrange
            var command = new UpdateProductCommand { Id = 1, PartNumber = "123", Name = "Updated Product" };
            var product = new Product { Id = 1, PartNumber = "456", Name = "Existing Product" };

            _productRepositoryMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(r => r.Update(It.IsAny<Product>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == nameof(UpdateProductCommand) && n.Message == "Error occurred while updating product"), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
