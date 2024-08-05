using InventoryControl.Application.Queries.Consumptions;
using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Repositories;
using InventoryControl.Infra.Data.Abstractions;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InventoryUnitTests.Handlers
{
    public class GetProductDailyConsumptionQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Consumption>> _consumptionRepositoryMock;
        private readonly GetProductDailyConsumptionQueryHandler _handler;

        public GetProductDailyConsumptionQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _consumptionRepositoryMock = new Mock<IRepository<Consumption>>();
            _handler = new GetProductDailyConsumptionQueryHandler(_unitOfWorkMock.Object);

            _unitOfWorkMock.SetupGet(u => u.Consumptions).Returns(_consumptionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnConsumption_WhenConsumptionExists()
        {
            // Arrange
            var query = new GetProductDailyConsumptionQuery(1, DateTime.UtcNow);
            var consumption = new Consumption { ProductId = 1, ConsumptionDate = DateTime.UtcNow, QuantityConsumed = 10, TotalAveragePrice = 2.0m, TotalCost = 20.0m };

            _consumptionRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Consumption, bool>>>()))
                .ReturnsAsync(consumption);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(consumption.ProductId, result.ProductId);
            Assert.Equal(consumption.ConsumptionDate, result.ConsumptionDate);
            Assert.Equal(consumption.QuantityConsumed, result.QuantityConsumed);
            Assert.Equal(consumption.TotalAveragePrice, result.TotalAveragePrice);
            Assert.Equal(consumption.TotalCost, result.TotalCost);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenConsumptionDoesNotExist()
        {
            // Arrange
            var query = new GetProductDailyConsumptionQuery(1, DateTime.UtcNow);

            _consumptionRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Consumption, bool>>>()))
                .ReturnsAsync((Consumption)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
