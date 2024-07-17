namespace Athena.Application.Features.DashboardFeatures.Notifications.Dtos
{
    public class NotificationTypeTemplateDto : IDto
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? Language { get; set; }
    }
}
