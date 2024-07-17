using Athena.Domain.Common.Const;

namespace Athena.Application.Features.NotificatioinFeatures.Spec
{
    public class LastNotificationRecipientByUserIdSpec : Specification<NotificationRecipient>
    {
        public LastNotificationRecipientByUserIdSpec(Guid userId) =>
            Query.Where(e => e.UserId == userId && e.Status == ENotificationStatus.UnSeen.ToString())
                .Include(e => e.Notification).ThenInclude(e => e.NotificationType)
                .Include(e => e.Notification).ThenInclude(e => e.NotificationMessages)
                .OrderByDescending(e => e.CreatedOn);
    }
}
