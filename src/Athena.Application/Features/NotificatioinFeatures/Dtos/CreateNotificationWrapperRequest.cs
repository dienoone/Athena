namespace Athena.Application.Features.NotificatioinFeatures.Dtos
{
    public class CreateNotificationWrapperRequest
    {
        public string Type { get; set; } = default!;
        public string Label { get; set; } = default!;
        public Guid? EntityId { get; set; }
        public Guid NotifierId { get; set; }
        public Guid BusinessId { get; set; }
        public string? Image { get; set; }
        public string? ArMessage { get; set; }
        public string? EnMessage { get; set; }
    }
}
