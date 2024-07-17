using Athena.Application.Features.NotificatioinFeatures.Dtos;

namespace Athena.Application.Common.Interfaces
{
    public interface INotificationService : ITransientService
    {
        Task<NotificationDto> CreateNotficationOnePlaceHolderAsync(CreateNotificationWrapperRequest notificationRequest, CancellationToken cancellationToken);
        Task<NotificationDto> CreateNotificationTwoPlaceHoldersAsync(CreateNotificationWrapperRequest notificationRequest, CancellationToken cancellationToken);
        Task<NotificationDto> CreateNotificationThreePlaceHoldersAsync(CreateNotificationWrapperRequest notificationRequest, CancellationToken cancellationToken);

        // ConnectionIds
        Task<IEnumerable<string>> CreateNotficationRecipientsAsync(IEnumerable<Guid> recipientIds, Guid notificationId, Guid businessId, CancellationToken cancellationToken);
        Task<IEnumerable<string>> CreateNotficationRecipientAsync(Guid recipientId, Guid notificationId, Guid businessId, CancellationToken cancellationToken);
        
    }
}
