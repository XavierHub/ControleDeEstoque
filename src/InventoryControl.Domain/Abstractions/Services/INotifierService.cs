namespace InventoryControl.Domain.Abstractions.Services
{
    public interface INotifierService
    {
        IReadOnlyCollection<Notification> Notifications();
        bool HasNotifications();
        void Add(string key, string message);
    }
}
