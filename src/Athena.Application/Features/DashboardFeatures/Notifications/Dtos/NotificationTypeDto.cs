namespace Athena.Application.Features.DashboardFeatures.Notifications.Dtos
{
    public class NotificationTypeDto : IDto
    {
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }

        public List<NotificationTypeTemplateDto>? Templates { get; set; }
    }
}
