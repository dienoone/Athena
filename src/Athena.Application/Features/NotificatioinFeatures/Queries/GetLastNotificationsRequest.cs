using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.NotificatioinFeatures.Spec;

namespace Athena.Application.Features.NotificatioinFeatures.Queries
{
    public record GetLastNotificationsRequest(Guid UserId) : IRequest<List<NotificationDto>>;

    public class GetLastNotificationsRequestHandler : IRequestHandler<GetLastNotificationsRequest, List<NotificationDto>>
    {
        private readonly IReadRepository<NotificationRecipient> _notificationRecipientRepo;
        private readonly ILanguageService _languageService;
        private readonly ILogger<GetLastNotificationsRequestHandler> _logger;

        public GetLastNotificationsRequestHandler(
            ILogger<GetLastNotificationsRequestHandler> logger,
            ILanguageService languageService,
            IReadRepository<NotificationRecipient> notificationRecipientRepo)
        {
            _logger = logger;
            _notificationRecipientRepo = notificationRecipientRepo;
            _languageService = languageService;
        }

        public async Task<List<NotificationDto>> Handle(GetLastNotificationsRequest request, CancellationToken cancellationToken)
        {
            List<NotificationDto> dtos = new();
            var notifications = await _notificationRecipientRepo.ListAsync(new LastNotificationRecipientByUserIdSpec(request.UserId), cancellationToken);

            _logger.LogInformation($"Language: {_languageService.GetCurrentLanguage()}");
            foreach(var notification in notifications)
            {
                NotificationDto dto = notification.Adapt<NotificationDto>();
                var message = notification.Notification.NotificationMessages.FirstOrDefault(e => e.Language == _languageService.GetCurrentLanguage());
                dto.Message = message!.Message;
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
