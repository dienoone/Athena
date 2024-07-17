namespace Athena.Application.Features.NotificatioinFeatures.Spec
{
    public class NotificationRecipientByUserIdAndNotificationIdSpec : Specification<NotificationRecipient>, ISingleResultSpecification
    {
        public NotificationRecipientByUserIdAndNotificationIdSpec(Guid userId, Guid notificationId) =>
            Query.Where(e => e.UserId == userId && e.NotificationId == notificationId);
    }
}
