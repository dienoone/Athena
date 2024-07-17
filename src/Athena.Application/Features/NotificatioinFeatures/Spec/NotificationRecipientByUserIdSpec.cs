namespace Athena.Application.Features.NotificatioinFeatures.Spec
{
    public class NotificationRecipientByUserIdSpec : Specification<NotificationRecipient>
    {
        public NotificationRecipientByUserIdSpec(Guid userId) =>
            Query.Where(e => e.UserId == userId && e.DeletedOn == null)
                .Include(e => e.Notification).ThenInclude(e => e.NotificationType)
                .Include(e => e.Notification).ThenInclude(e => e.NotificationMessages)
                .OrderByDescending(e => e.CreatedOn);
            
    }
}
