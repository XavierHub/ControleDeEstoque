using InventoryControl.Application.Queries.Consumptions;
using InventoryControl.Domain;
using InventoryControl.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryUnitTests.Controllers
{
    public class ConsumptionControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsumptionController _controller;

        public ConsumptionControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ConsumptionController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetDailyConsumption_ReturnsConsumption_ForSpecificDate()
        {
            // Arrange
            int productId = 1;
            DateTime date = new DateTime(2023, 8, 1);
            var consumption = new Consumption { Id = 1, ProductId = productId, ConsumptionDate = date, QuantityConsumed = 5 };
            _mediatorMock.Setup(m => m.Send(It.Is<GetProductDailyConsumptionQuery>(q => q.ProductId == productId && q.Date == date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(consumption);

            // Act
            var result = await _controller.GetDailyConsumption(productId, date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Consumption>(okResult.Value);
            Assert.Equal(consumption, returnValue);
        }

        [Fact]
        public async Task GetDailyConsumption_ReturnsConsumption_ForCurrentDate()
        {
            // Arrange
            int productId = 1;
            DateTime currentDate = DateTime.Now.Date;
            var consumption = new Consumption { Id = 1, ProductId = productId, ConsumptionDate = currentDate, QuantityConsumed = 5 };
            _mediatorMock.Setup(m => m.Send(It.Is<GetProductDailyConsumptionQuery>(q => q.ProductId == productId && q.Date == currentDate), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(consumption);

            // Act
            var result = await _controller.GetDailyConsumption(productId, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Consumption>(okResult.Value);
            Assert.Equal(consumption, returnValue);
        }

        [Fact]
        public async Task GetDailyConsumption_ReturnsNotFound_WhenNoConsumptionFound()
        {
            // Arrange
            int productId = 1;
            DateTime date = new DateTime(2023, 8, 1);
            _mediatorMock.Setup(m => m.Send(It.Is<GetProductDailyConsumptionQuery>(q => q.ProductId == productId && q.Date == date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Consumption());

            // Act
            var result = await _controller.GetDailyConsumption(productId, date);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetDailyConsumption_ThrowsException()
        {
            // Arrange
            int productId = 1;
            DateTime date = new DateTime(2023, 8, 1);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductDailyConsumptionQuery>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _controller.GetDailyConsumption(productId, date));

            // Assert
            Assert.Equal("Test exception", result.Message);
        }
    }
}
