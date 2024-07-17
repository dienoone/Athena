namespace Athena.Domain.Entities
{
    public class NotificationMessage : AuditableEntity, IAggregateRoot
    {
        public string Language { get; private set; } = default!;
        public string Message { get; private set; } = default!;

        public Guid NotificationId { get; private set; }
        public virtual Notification Notification { get; private set; } = default!;

        public NotificationMessage(string language, string message, Guid notificationId, Guid businessId)
        {
            Language = language;
            Message = message; 
            NotificationId = notificationId;
            BusinessId = businessId;
        }
    }
}
