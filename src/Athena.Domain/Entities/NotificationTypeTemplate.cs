namespace Athena.Domain.Entities
{
    public class NotificationTypeTemplate : BaseEntity, IAggregateRoot
    {
        public string Content { get; private set; } = default!;
        public string Language { get; private set; } = default!;
        public bool IsDeleted { get; private set; }
        public Guid NotificationTypeId { get; private set; }
        public NotificationType NotificationType { get; private set; } = null!; 

        public NotificationTypeTemplate(string content, string language, bool isDeleted, Guid notificationTypeId)
        {
            Content = content;
            Language = language;
            IsDeleted = isDeleted;
            NotificationTypeId = notificationTypeId;

        }

        public NotificationTypeTemplate Update(string? content, string? language, bool? isDeleted)
        {
            if (content is not null && Content?.Equals(content) is not true) Content = content;
            if (language is not null && Language?.Equals(language) is not true) Language = language;
            if (isDeleted is not null && IsDeleted != isDeleted) IsDeleted = (bool)isDeleted;
            return this;
        }
    }
}
