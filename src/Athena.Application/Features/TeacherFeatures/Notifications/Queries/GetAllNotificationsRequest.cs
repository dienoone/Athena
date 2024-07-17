using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.TeacherFeatures.Notifications.Spec;

namespace Athena.Application.Features.TeacherFeatures.Notifications.Queries
{
    public record GetAllNotificationsRequest() : IRequest<List<NotificationDto>>;

    public class GetAllNotificationsRequestHandler : IRequestHandler<GetAllNotificationsRequest, List<NotificationDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly ILanguageService _languageService;
        private readonly IReadRepository<NotificationRecipient> _notificationRecipientRepo;

        public GetAllNotificationsRequestHandler(
            ICurrentUser currentUser, 
            ILanguageService languageService,
            IReadRepository<NotificationRecipient> notificationRecipientRepo)
        {
            _currentUser = currentUser;
            _languageService = languageService;
            _notificationRecipientRepo = notificationRecipientRepo;
        }

        public async Task<List<NotificationDto>> Handle(GetAllNotificationsRequest request, CancellationToken cancellationToken)
        {
            List<NotificationDto> dtos = new();
            var notifications = await _notificationRecipientRepo.ListAsync(new NotificationRecipientByUserIdAndLanguageSpec(_currentUser.GetUserId(), _languageService.GetCurrentLanguage()), cancellationToken);
            foreach (var notification in notifications)
            {
                var notificationDto = MapNotification(notification);
                dtos.Add(notificationDto);
            }
            
            return dtos;
        }

        private static NotificationDto MapNotification(NotificationRecipient notificationRecipient)
        {
            return new()
            {
                Id = notificationRecipient.Id,
                Type = notificationRecipient.Notification.NotificationType.Type,
                Message = notificationRecipient.Notification.NotificationMessages.FirstOrDefault()!.Message,
                Status = notificationRecipient.Status,
                NotificationLabel = notificationRecipient.Notification.NotificationLabel,
                EntityId = notificationRecipient.Notification.EntityId,
                Image = notificationRecipient.Notification.Image,
                CreatedOn = notificationRecipient.Notification.CreatedOn,
            };
        }
    }
}
