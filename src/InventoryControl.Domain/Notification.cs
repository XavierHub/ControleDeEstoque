namespace InventoryControl.Domain
{
    public class Notification
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public Exception? Exception { get; set; }

        public Notification(string key, string message, Exception? exception = null)
        {
            Key = key;
            Message = message;
            Exception = exception;
        }
    }
}
