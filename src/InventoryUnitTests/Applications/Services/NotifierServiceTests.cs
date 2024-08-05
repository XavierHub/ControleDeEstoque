using EmpXpo.Accounting.Application.Services;
using InventoryControl.Domain;
using System.Collections.Generic;
using Xunit;

namespace InventoryUnitTests.Services
{
    public class NotifierServiceTests
    {
        private readonly NotifierService _notifierService;

        public NotifierServiceTests()
        {
            _notifierService = new NotifierService();
        }

        [Fact]
        public void HasNotifications_ShouldReturnFalse_WhenNoNotificationsExist()
        {
            // Act
            var result = _notifierService.HasNotifications();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasNotifications_ShouldReturnTrue_WhenNotificationsExist()
        {
            // Arrange
            _notifierService.Add("TestKey", "Test message");

            // Act
            var result = _notifierService.HasNotifications();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Notifications_ShouldReturnEmptyCollection_WhenNoNotificationsExist()
        {
            // Act
            var result = _notifierService.Notifications();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Notifications_ShouldReturnAllNotifications_WhenNotificationsExist()
        {
            // Arrange
            var notification1 = new Notification("Key1", "Message1");
            var notification2 = new Notification("Key2", "Message2");

            _notifierService.Add(notification1.Key, notification1.Message);
            _notifierService.Add(notification2.Key, notification2.Message);

            // Act
            var result = _notifierService.Notifications();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, n => n.Key == "Key1" && n.Message == "Message1");
            Assert.Contains(result, n => n.Key == "Key2" && n.Message == "Message2");
        }

        [Fact]
        public void Add_ShouldAddNotification()
        {
            // Arrange
            var key = "NewKey";
            var message = "New message";

            // Act
            _notifierService.Add(key, message);
            var result = _notifierService.Notifications();

            // Assert
            Assert.NotEmpty(result);
            Assert.Contains(result, n => n.Key == key && n.Message == message);
        }
    }
}
