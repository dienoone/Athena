namespace Athena.Domain.Entities
{
    public class Notification : AuditableEntity, IAggregateRoot
    {
        public string NotificationLabel { get; private set; } = default!;
        public Guid? EntityId { get; private set; }
        public Guid NotifierId { get; private set; }
        public string? Image { get; private set; }
        public Guid NotificationTypeId { get; private set; }
        public NotificationType NotificationType { get; private set; } = null!;

        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; private set; } = null!;
        public virtual ICollection<NotificationMessage> NotificationMessages { get; private set; } = null!;

        public Notification(string notificationLabel, Guid? entityId, Guid notifierId,
            string? image, Guid notificationTypeId, Guid businessId)
        {
            NotificationLabel = notificationLabel;
            EntityId = entityId;
            NotifierId = notifierId;
            Image = image;
            NotificationTypeId = notificationTypeId;
            BusinessId = businessId;
        }

    }
}
