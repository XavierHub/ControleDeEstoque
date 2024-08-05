using InventoryControl.Domain;
using MediatR;

namespace InventoryControl.Application.Notifications
{
    public class NotificationCommand : Notification, INotification
    {
        public NotificationCommand(string key, string message, Exception? exception = null) : base(key, message, exception)
        {
        }
    }
}
