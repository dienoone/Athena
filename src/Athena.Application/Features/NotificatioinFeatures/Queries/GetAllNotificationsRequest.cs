using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.NotificatioinFeatures.Spec;

namespace Athena.Application.Features.NotificatioinFeatures.Queries
{
    public record GetAllNotificationsRequest() : IRequest<List<NotificationDto>>;

    public class GetAllNotificationsRequestHandler : IRequestHandler<GetAllNotificationsRequest, List<NotificationDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<NotificationRecipient> _notificationRecipientRepo;
        private readonly ILanguageService _languageService;

        public GetAllNotificationsRequestHandler(
            ICurrentUser currentUser, 
            IReadRepository<NotificationRecipient> notificationRecipientRepo,
            ILanguageService languageService)
        {
            _currentUser = currentUser;
            _notificationRecipientRepo = notificationRecipientRepo;
            _languageService = languageService;
        }

        public async Task<List<NotificationDto>> Handle(GetAllNotificationsRequest request, CancellationToken cancellationToken)
        {
            List<NotificationDto> dtos = new();
            var messages = await _notificationRecipientRepo.ListAsync(
                new NotificationRecipientByUserIdSpec(_currentUser.GetUserId()), cancellationToken);

            var language = _languageService.GetCurrentLanguage();
            Console.WriteLine($"Language: {language}");

            foreach (var message in messages)
            {
                var messageDto = message.Adapt<NotificationDto>();
                messageDto.Message = message.Notification.NotificationMessages.FirstOrDefault(e => e.Language == "ar")?.Message;
                dtos.Add(messageDto);
            }
            return dtos;
        }
    }
}
