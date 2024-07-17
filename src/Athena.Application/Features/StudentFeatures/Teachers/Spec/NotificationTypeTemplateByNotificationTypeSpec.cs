namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class NotificationTypeTemplateByNotificationTypeSpec : Specification<NotificationTypeTemplate>
    {
        public NotificationTypeTemplateByNotificationTypeSpec(string notificationType) =>
            Query.Where(e => e.NotificationType.Type == notificationType);
    }
}
