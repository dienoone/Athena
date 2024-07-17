namespace Athena.Application.Features.NotificatioinFeatures.Dtos
{
    public class NotificationDto : IDto
    {
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
        public string? NotificationLabel { get; set; }
        public Guid? EntityId { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
