namespace Athena.Domain.Entities
{
    public class NotificationType : BaseEntity, IAggregateRoot
    {
        public string Type { get; private set; } = default!;
        public string? Description { get; private set; }
        public bool IsDeleted { get; private set; }
        public virtual ICollection<Notification> Notifications { get; private set; } = null!;
        public virtual ICollection<NotificationTypeTemplate> NotificationTypeTemplates { get; private set; } = null!;


        public NotificationType(string type, string? description, bool isDeleted)
        {
            Type = type;
            Description = description;
            IsDeleted = isDeleted;

        }

        public NotificationType Update(string? type, string? description, bool? isDeleted)
        {
            if (type is not null && Type?.Equals(type) is not true) Type = type;
            if (description is not null && Description?.Equals(description) is not true) Description = description;
            if (isDeleted is not null && IsDeleted != isDeleted) IsDeleted = (bool)isDeleted;
            return this;
        }

    }
}
