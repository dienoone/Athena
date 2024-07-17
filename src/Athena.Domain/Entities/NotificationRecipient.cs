namespace Athena.Domain.Entities
{
    public class NotificationRecipient : AuditableEntity, IAggregateRoot
    {
        public string Status { get; private set; } = default!;
        public Guid UserId { get; private set; }
        public Guid NotificationId { get; private set; }
        public Notification Notification { get; private set; } = null!;

        public NotificationRecipient(string status, Guid userId, Guid notificationId, Guid businessId)
        {
            Status = status;
            UserId = userId;
            NotificationId = notificationId;
            BusinessId = businessId;
        }

        public NotificationRecipient Update(string status)
        {
            if (status is not null && Status?.Equals(status) is not true) Status = status;
            return this;
        }


    }
}
