namespace Athena.Application.Features.NotificatioinFeatures.Spec
{
    public class NotificationByNotifierIdAndNotificationIdSpec : Specification<Notification>, ISingleResultSpecification
    {
        public NotificationByNotifierIdAndNotificationIdSpec(Guid notifierId, Guid id) =>
            Query.Where(e => e.NotifierId == notifierId && e.Id == id);
    }
}
