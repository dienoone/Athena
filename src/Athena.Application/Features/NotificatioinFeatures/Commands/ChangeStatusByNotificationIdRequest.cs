using Athena.Application.Features.NotificatioinFeatures.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.NotificatioinFeatures.Commands
{
    public record ChangeStatusByNotificationIdRequest(Guid Id, Guid UserId) : IRequest<Guid>;

    public class ChangeStatusByNotificationIdRequestHandler : IRequestHandler<ChangeStatusByNotificationIdRequest, Guid>
    {
        private readonly IRepository<NotificationRecipient> _notificationRecipientRepo;
        private readonly IStringLocalizer<ChangeStatusByNotificationIdRequestHandler> _t;

        public ChangeStatusByNotificationIdRequestHandler(
            IRepository<NotificationRecipient> notificationRecipientRepo, 
            IStringLocalizer<ChangeStatusByNotificationIdRequestHandler> t)
        {
            _notificationRecipientRepo = notificationRecipientRepo;
            _t = t;
        }

        public async Task<Guid> Handle(ChangeStatusByNotificationIdRequest request, CancellationToken cancellationToken)
        {
            var notificationRecipient = await _notificationRecipientRepo.GetBySpecAsync(new NotificationRecipientByUserIdAndNotificationIdSpec(request.UserId, request.Id), cancellationToken);
            _ = notificationRecipient ?? throw new NotFoundException(_t["Notification {0} Not Found!", request.Id]);

            notificationRecipient.Update(ENotificationStatus.Read.ToString());
            await _notificationRecipientRepo.UpdateAsync(notificationRecipient, cancellationToken);

            return request.Id;
        }
    }
}
