namespace Athena.Application.Features.TeacherFeatures.Notifications.Spec
{
    public class NotificationRecipientByUserIdAndLanguageSpec : Specification<NotificationRecipient>
    {
        public NotificationRecipientByUserIdAndLanguageSpec(Guid userId, string language) =>
            Query.Where(e => e.UserId == userId)
                .Include(e => e.Notification).ThenInclude(n => n.NotificationType)
                .Include(e => e.Notification.NotificationMessages.Where(m => m.Language == language && m.DeletedOn == null));
                
    }
}
