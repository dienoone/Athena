namespace Athena.Application.Features.DashboardFeatures.Notifications.Commands
{
    public record DeleteNotificationTypeByIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteNotificationTypeByIdRequestHandler : IRequestHandler<DeleteNotificationTypeByIdRequest, Guid>
    {
        private readonly IRepository<NotificationType> _notificationTypeRepo;
        private readonly IStringLocalizer<DeleteNotificationTypeByIdRequestHandler> _t;

        public DeleteNotificationTypeByIdRequestHandler(IRepository<NotificationType> notificationTypeRepo, IStringLocalizer<DeleteNotificationTypeByIdRequestHandler> t)
        {
            _notificationTypeRepo = notificationTypeRepo;
            _t = t;
        }

        public async Task<Guid> Handle(DeleteNotificationTypeByIdRequest request, CancellationToken cancellationToken)
        {
            var notificationType = await _notificationTypeRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = notificationType ?? throw new NotFoundException(_t["NotificationType {0} Not Found!", request.Id]);

            await _notificationTypeRepo.DeleteAsync(notificationType, cancellationToken);
            return request.Id;
        }
    }
}
