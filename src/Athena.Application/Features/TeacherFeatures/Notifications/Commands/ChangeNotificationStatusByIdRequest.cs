using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Notifications.Commands
{
    public record ChangeNotificationStatusByIdRequest(Guid Id) : IRequest<Guid>;

    public class ChangeNotificationStatusByIdRequestHandler : IRequestHandler<ChangeNotificationStatusByIdRequest, Guid>
    {
        private readonly IRepository<NotificationRecipient> _notificationRecipientRepo;
        private readonly IStringLocalizer<ChangeNotificationStatusByIdRequestHandler> _t;

        public ChangeNotificationStatusByIdRequestHandler(
            IRepository<NotificationRecipient> notificationRecipientRepo, 
            IStringLocalizer<ChangeNotificationStatusByIdRequestHandler> t)
        {
            _notificationRecipientRepo = notificationRecipientRepo;
            _t = t;
        }

        public async Task<Guid> Handle(ChangeNotificationStatusByIdRequest request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRecipientRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = notification ?? throw new NotFoundException(_t["Notification {0} Not Found!", request.Id]);

            notification.Update(ENotificationStatus.Read.ToString());
            await _notificationRecipientRepo.UpdateAsync(notification, cancellationToken);

            return notification.Id;
        }
    }
}
