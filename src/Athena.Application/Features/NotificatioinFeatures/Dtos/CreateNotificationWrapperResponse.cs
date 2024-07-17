namespace Athena.Application.Features.NotificatioinFeatures.Dtos
{
    public class CreateNotificationWrapperResponse
    {
        public Guid Id { get; set; }
        public NotificationDto NotificationDto { get; set; } = default!;
    }
}
