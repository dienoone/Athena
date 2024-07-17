using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Application.Features.TeacherFeatures.Groups.Spec;
using Athena.Domain.Common.Const;
using Athena.Infrastructure.Notifications.Spec;

namespace Athena.Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly ILanguageService _languageService;
        private readonly IReadRepository<NotificationTypeTemplate> _notificationTypeTemplateRepo;
        private readonly IRepository<Notification> _notificationRepo;
        private readonly IRepository<NotificationMessage> _notificationMessageRepo;
        private readonly IRepository<NotificationRecipient> _notificationRecipientRepo;
        private readonly IReadRepository<SignalRConnection> _signalRConnectionRepo;

        public NotificationService(
            ILanguageService languageService, 
            IReadRepository<NotificationTypeTemplate> notificationTypeTemplateRepo, 
            IRepository<Notification> notificationRepo, 
            IRepository<NotificationMessage> notificationMessageRepo, 
            IRepository<NotificationRecipient> notificationRecipientRepo, 
            IReadRepository<SignalRConnection> signalRConnectionRepo)
        {
            _languageService = languageService;
            _notificationTypeTemplateRepo = notificationTypeTemplateRepo;
            _notificationRepo = notificationRepo;
            _notificationMessageRepo = notificationMessageRepo;
            _notificationRecipientRepo = notificationRecipientRepo;
            _signalRConnectionRepo = signalRConnectionRepo;
        }

        public async Task<NotificationDto> CreateNotficationOnePlaceHolderAsync(CreateNotificationWrapperRequest notificationRequest, CancellationToken cancellationToken)
        {
            var templates = await _notificationTypeTemplateRepo.ListAsync(new NotificationTypeTemplateByNotificationTypeSpec(notificationRequest.Type), cancellationToken);
            var arabicTemplate = templates.FirstOrDefault(e => e.Language == "ar");
            var englishTemplate = templates.FirstOrDefault(e => e.Language == "en");

            Notification notification = new(
                notificationRequest.Label, 
                notificationRequest.EntityId, 
                notificationRequest.NotifierId, 
                notificationRequest.Image, 
                arabicTemplate!.NotificationTypeId, 
                notificationRequest.BusinessId);

            await _notificationRepo.AddAsync(notification, cancellationToken);

            NotificationMessage notificationMessageAr = new(
                Languages.Arabic, 
                notificationRequest.ArMessage != null ? arabicTemplate.Content.Replace("{holder}", notificationRequest.ArMessage) : arabicTemplate.Content,
                notification.Id, 
                notification.BusinessId);

            NotificationMessage notificationMessageEn = new(
                Languages.English,
                notificationRequest.EnMessage != null ? englishTemplate!.Content.Replace("{holder}", notificationRequest.EnMessage) : englishTemplate!.Content, 
                notification.Id, 
                notification.BusinessId);

            await _notificationMessageRepo.AddAsync(notificationMessageEn, cancellationToken);
            await _notificationMessageRepo.AddAsync(notificationMessageAr, cancellationToken);

            return new() 
            {
                Id = notification.Id,
                Type = notificationRequest.Type,
                Message = _languageService.GetCurrentLanguage() == "ar" ? notificationMessageAr.Message : notificationMessageEn.Message,
                Status = ENotificationStatus.UnSeen.ToString(),
                NotificationLabel = notification.NotificationLabel,
                EntityId = notificationRequest.EntityId,
                Image = notification.Image,
                CreatedOn = notification.CreatedOn
            };
        }

        public async Task<NotificationDto> CreateNotificationTwoPlaceHoldersAsync(CreateNotificationWrapperRequest notificationRequest, CancellationToken cancellationToken)
        {
            var templates = await _notificationTypeTemplateRepo.ListAsync(new NotificationTypeTemplateByNotificationTypeSpec(notificationRequest.Type), cancellationToken);
            var arabicTemplate = templates.FirstOrDefault(e => e.Language == "ar");
            var englishTemplate = templates.FirstOrDefault(e => e.Language == "en");

            Notification notification = new(
                notificationRequest.Label,
                notificationRequest.EntityId,
                notificationRequest.NotifierId,
                notificationRequest.Image,
                arabicTemplate!.NotificationTypeId,
                notificationRequest.BusinessId);

            await _notificationRepo.AddAsync(notification, cancellationToken);

            NotificationMessage notificationMessageAr = new(
                Languages.Arabic, 
                arabicTemplate.Content
                .Replace("{holder1}", notificationRequest.ArMessage?.Split("&&")[0])
                .Replace("{holder2}", notificationRequest.ArMessage?.Split("&&")[1]), 
                notification.Id, notification.BusinessId);

            NotificationMessage notificationMessageEn = new(
                Languages.English, 
                englishTemplate!.Content
                .Replace("{holder1}", notificationRequest.EnMessage?.Split("&&")[0])
                .Replace("{holder2}", notificationRequest.EnMessage?.Split("&&")[1]), 
                notification.Id, notification.BusinessId);

            await _notificationMessageRepo.AddAsync(notificationMessageEn, cancellationToken);
            await _notificationMessageRepo.AddAsync(notificationMessageAr, cancellationToken);

            return new()
            {
                Id = notification.Id,
                Type = notificationRequest.Type,
                Message = _languageService.GetCurrentLanguage() == "ar" ? notificationMessageAr.Message : notificationMessageEn.Message,
                Status = ENotificationStatus.UnSeen.ToString(),
                NotificationLabel = notification.NotificationLabel,
                EntityId = notificationRequest.EntityId,
                Image = notification.Image,
                CreatedOn = notification.CreatedOn
            };
        }

        public async Task<NotificationDto> CreateNotificationThreePlaceHoldersAsync(CreateNotificationWrapperRequest notificationRequest, CancellationToken cancellationToken)
        {
            var templates = await _notificationTypeTemplateRepo.ListAsync(new NotificationTypeTemplateByNotificationTypeSpec(notificationRequest.Type), cancellationToken);
            var arabicTemplate = templates.FirstOrDefault(e => e.Language == "ar");
            var englishTemplate = templates.FirstOrDefault(e => e.Language == "en");

            Notification notification = new(
                notificationRequest.Label,
                notificationRequest.EntityId,
                notificationRequest.NotifierId,
                notificationRequest.Image,
                arabicTemplate!.NotificationTypeId,
                notificationRequest.BusinessId);

            await _notificationRepo.AddAsync(notification, cancellationToken);

            NotificationMessage notificationMessageAr = new(
                Languages.Arabic,
                arabicTemplate.Content
                .Replace("{holder1}", notificationRequest.ArMessage?.Split("&&")[0])
                .Replace("{holder2}", notificationRequest.ArMessage?.Split("&&")[1])
                .Replace("{holder3}", notificationRequest.ArMessage?.Split("&&")[2]),
                notification.Id, notification.BusinessId);

            NotificationMessage notificationMessageEn = new(
                Languages.English,
                englishTemplate!.Content
                .Replace("{holder1}", notificationRequest.EnMessage?.Split("&&")[0])
                .Replace("{holder2}", notificationRequest.EnMessage?.Split("&&")[1])
                .Replace("{holder3}", notificationRequest.EnMessage?.Split("&&")[2]),
                notification.Id, notification.BusinessId);

            await _notificationMessageRepo.AddAsync(notificationMessageEn, cancellationToken);
            await _notificationMessageRepo.AddAsync(notificationMessageAr, cancellationToken);

            return new()
            {
                Id = notification.Id,
                Type = notificationRequest.Type,
                Message = _languageService.GetCurrentLanguage() == "ar" ? notificationMessageAr.Message : notificationMessageEn.Message,
                Status = ENotificationStatus.UnSeen.ToString(),
                NotificationLabel = notification.NotificationLabel,
                EntityId = notificationRequest.EntityId,
                Image = notification.Image,
                CreatedOn = notification.CreatedOn
            };
        }

        // this function return connectionIds for connected users signalR:
        public async Task<IEnumerable<string>> CreateNotficationRecipientAsync(Guid recipientId, Guid notificationId, Guid businessId, CancellationToken cancellationToken)
        {
            NotificationRecipient notificationRecipient = new(ENotificationStatus.UnSeen.ToString(), recipientId, notificationId, businessId);
            await _notificationRecipientRepo.AddAsync(notificationRecipient, cancellationToken);

            var connections = await _signalRConnectionRepo.ListAsync(new SignalRConnectionsByUserIdSpec(recipientId), cancellationToken);
            return connections.Select(e => e.ConnectionId);
        }

        // this function return connectionIds for connected users signalR:
        public async Task<IEnumerable<string>> CreateNotficationRecipientsAsync(IEnumerable<Guid> recipientIds, Guid notificationId, Guid businessId, CancellationToken cancellationToken)
        {

            foreach (var recipientId in recipientIds)
            {
                NotificationRecipient notificationRecipient = new(ENotificationStatus.UnSeen.ToString(), recipientId, notificationId, businessId);
                await _notificationRecipientRepo.AddAsync(notificationRecipient, cancellationToken);
            }

            var connections = await _signalRConnectionRepo.ListAsync(new GetSignalRConnectionIdsByUserIdsSpec(recipientIds), cancellationToken);
            return connections.Select(e => e.ConnectionId);
        }
    }
}
