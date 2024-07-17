namespace Athena.Application.Features.DashboardFeatures.Notifications.Spec
{
    public class NotificationTypeTemplateByNotificationTypeIdAndLanguageSpec : Specification<NotificationTypeTemplate>, ISingleResultSpecification
    {
        public NotificationTypeTemplateByNotificationTypeIdAndLanguageSpec(Guid notificationTypeId, string Language) =>
            Query.Where(e => e.NotificationTypeId == notificationTypeId && e.Language == Language && e.IsDeleted == false);
    }
}
