namespace Athena.Application.Features.TeacherFeatures.Notifications.Commands
{
    public record DeleteNotificationRecipientsByIdsRequest(List<Guid> Ids) : IRequest<Guid>;
    
    public class DeleteNotificationRecipientsByIdsRequestValidator : CustomValidator<DeleteNotificationRecipientsByIdsRequest>
    {
        public DeleteNotificationRecipientsByIdsRequestValidator(
            IReadRepository<NotificationRecipient> notificationRecipientRepo,
            IStringLocalizer<DeleteNotificationRecipientsByIdsRequestValidator> T) 
        {
            RuleFor(e => e.Ids)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (request, ids, ct) => await CheckIds(ids, notificationRecipientRepo, T, ct));
        }

        private static async Task<bool> CheckIds(
            List<Guid> ids, 
            IReadRepository<NotificationRecipient> notificationRecipientRepo, 
            IStringLocalizer<DeleteNotificationRecipientsByIdsRequestValidator> T,
            CancellationToken cancellationToken)
        {
            if (!(ids.Count >= 1))
                throw new ConflictException(T["Notification List must has at least one notification to delete."]);

            foreach(var id in ids)
            {
                var notification = await notificationRecipientRepo.GetByIdAsync(id, cancellationToken);
                _ = notification ?? throw new NotFoundException(T["Notification {0} Not Found!", id]);
            }

            return true;
        }
    }

    public class DeleteNotificationRecipientsByIdsRequestHandler : IRequestHandler<DeleteNotificationRecipientsByIdsRequest, Guid>
    {
        private readonly IRepository<NotificationRecipient> _notificationRecipientRepo;

        public DeleteNotificationRecipientsByIdsRequestHandler(IRepository<NotificationRecipient> notificationRecipientRepo)
        {
            _notificationRecipientRepo = notificationRecipientRepo;
        }

        public async Task<Guid> Handle(DeleteNotificationRecipientsByIdsRequest request, CancellationToken cancellationToken)
        {
            foreach(var id in request.Ids)
            {
                var notification = await _notificationRecipientRepo.GetByIdAsync(id, cancellationToken);
                await _notificationRecipientRepo.DeleteAsync(notification!, cancellationToken);
            }

            return request.Ids[0];
        }
    }
}
