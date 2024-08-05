using InventoryControl.Application.Notifications;
using InventoryControl.Domain.Abstractions.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InventoryUnitTests.Handlers
{
    public class NotificationHandlerTests
    {
        private readonly Mock<ILogger<NotificationHandler>> _loggerMock;
        private readonly Mock<INotifierService> _notifierServiceMock;
        private readonly NotificationHandler _handler;

        public NotificationHandlerTests()
        {
            _loggerMock = new Mock<ILogger<NotificationHandler>>();
            _notifierServiceMock = new Mock<INotifierService>();
            _handler = new NotificationHandler(_loggerMock.Object, _notifierServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldLogError_WhenExceptionIsNotNull()
        {
            // Arrange
            var notification = new NotificationCommand("TestKey", "Test message", new Exception("Test exception"));

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Key:TestKey|Message:Test message")),
                    It.Is<Exception>(e => e.Message == "Test exception"),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            _notifierServiceMock.Verify(ns => ns.Add("TestKey", "Test message"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldLogError_WhenExceptionIsNull()
        {
            // Arrange
            var notification = new NotificationCommand("TestKey", "Test message");

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Key:TestKey|Message:Test message")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            _notifierServiceMock.Verify(ns => ns.Add("TestKey", "Test message"), Times.Once);
        }
    }
}
