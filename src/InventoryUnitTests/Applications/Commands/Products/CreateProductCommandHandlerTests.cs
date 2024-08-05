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
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new CreateProductCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);

            _unitOfWorkMock.SetupGet(u => u.Products).Returns(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_WhenPartNumberIsUnique()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                PartNumber = "123"
            };

            _productRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Product>());

            _productRepositoryMock.Setup(r => r.Insert(It.IsAny<Product>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result);
            _unitOfWorkMock.Verify(u => u.BeginTransaction(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnDefault_WhenPartNumberIsNotUnique()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                PartNumber = "123"
            };

            _productRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(new[] { new Product { PartNumber = "123" } });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(default, result);
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "PartNumber"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldRollbackTransaction_AndPublishNotification_WhenExceptionOccurs()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                PartNumber = "123"
            };

            _productRepositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Product>());

            _productRepositoryMock.Setup(r => r.Insert(It.IsAny<Product>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(default, result);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "Product" && n.Message.Contains("An error occurred while creating a product")), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
