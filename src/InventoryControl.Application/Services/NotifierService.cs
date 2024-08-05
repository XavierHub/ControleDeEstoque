using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Services;

namespace EmpXpo.Accounting.Application.Services
{
    public class NotifierService : INotifierService
    {
        private readonly List<Notification> _notifications;

        public NotifierService()
        {
            _notifications = new List<Notification>();
        }
        public bool HasNotifications() => _notifications.Any();
        public IReadOnlyCollection<Notification> Notifications() => _notifications;
        public void Add(string key, string message) => _notifications.Add(new Notification(key, message));
    }
}
