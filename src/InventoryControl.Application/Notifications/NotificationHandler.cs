using InventoryControl.Domain.Abstractions.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryControl.Application.Notifications
{
    public class NotificationHandler : INotificationHandler<NotificationCommand>
    {
        private readonly ILogger<NotificationHandler> _logger;
        private readonly INotifierService _notifierService;

        public NotificationHandler(ILogger<NotificationHandler> logger, INotifierService notifierService)
        {
            _logger = logger;
            _notifierService = notifierService;
        }

        public Task Handle(NotificationCommand notification, CancellationToken cancellationToken)
        {
            if (notification.Exception != null)
            {
                _logger.LogError(notification.Exception, "Key:{Key}|Message:{Message}", notification.Key, notification.Message);
            }
            else
            {
                _logger.LogError("Key:{Key}|Message:{Message}", notification.Key, notification.Message);
            }

            _notifierService.Add(notification.Key, notification.Message);

            return Task.CompletedTask;
        }
    }
}
