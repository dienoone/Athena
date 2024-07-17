namespace Athena.Application.Features.DashboardFeatures.Notifications.Spec
{
    public class NotificationTypeByTypeSpec : Specification<NotificationType>, ISingleResultSpecification
    {
        public NotificationTypeByTypeSpec(string type) =>
            Query.Where(e => e.Type == type && e.IsDeleted == false);
    }
}
