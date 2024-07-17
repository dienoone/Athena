namespace Athena.Application.Features.DashboardFeatures.Notifications.Spec
{
    public class NotificationTypesIncludeTemplatesSpec : Specification<NotificationType>
    {
        public NotificationTypesIncludeTemplatesSpec() =>
            Query.Include(e => e.NotificationTypeTemplates.Where(e => e.IsDeleted == false));
    }
}
