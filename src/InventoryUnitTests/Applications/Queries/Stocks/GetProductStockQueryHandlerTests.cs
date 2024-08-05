using InventoryControl.Application.Notifications;
using InventoryControl.Application.Queries.Stocks;
using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Repositories;
using InventoryControl.Infra.Data.Abstractions;
using MediatR;
using Moq;

namespace InventoryUnitTests.Handlers
{
    public class GetProductStockQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<IRepository<Stock>> _stockRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GetProductStockQueryHandler _handler;

        public GetProductStockQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _stockRepositoryMock = new Mock<IRepository<Stock>>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new GetProductStockQueryHandler(_unitOfWorkMock.Object, _mediatorMock.Object);

            _unitOfWorkMock.SetupGet(u => u.Products).Returns(_productRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Stocks).Returns(_stockRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnStockQuantity_WhenProductAndStockExist()
        {
            // Arrange
            var query = new GetProductStockQuery(1);
            var product = new Product { Id = 1, PartNumber = "123", Name = "Product 1" };
            var stock = new Stock { ProductId = 1, Quantity = 10 };

            _productRepositoryMock.Setup(r => r.GetById(query.ProductId))
                .ReturnsAsync(product);
            _stockRepositoryMock.Setup(r => r.Query(It.IsAny<System.Linq.Expressions.Expression<System.Func<Stock, bool>>>()))
                .ReturnsAsync(new[] { stock });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(stock.Quantity, result);
        }

        [Fact]
        public async Task Handle_ShouldReturnZero_WhenProductExistsButStockDoesNot()
        {
            // Arrange
            var query = new GetProductStockQuery( 1 );
            var product = new Product { Id = 1, PartNumber = "123", Name = "Product 1" };

            _productRepositoryMock.Setup(r => r.GetById(query.ProductId))
                .ReturnsAsync(product);
            _stockRepositoryMock.Setup(r => r.Query(It.IsAny<System.Linq.Expressions.Expression<System.Func<Stock, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Stock>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task Handle_ShouldReturnMinusOneAndPublishNotification_WhenProductDoesNotExist()
        {
            // Arrange
            var query = new GetProductStockQuery(1);

            _productRepositoryMock.Setup(r => r.GetById(query.ProductId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(-1, result);
            _mediatorMock.Verify(m => m.Publish(It.Is<NotificationCommand>(n => n.Key == "ProductNotFound" && n.Message.Contains("Product with ID 1 does not exist.")), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
