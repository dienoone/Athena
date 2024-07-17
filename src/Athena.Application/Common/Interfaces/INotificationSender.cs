using Athena.Application.Features.NotificatioinFeatures.Dtos;

namespace Athena.Application.Common.Interfaces
{
    public interface INotificationSender : ITransientService
    {
        Task BroadcastAsync(NotificationDto notification, CancellationToken cancellationToken);
        Task BroadcastAsync(NotificationDto notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken);

        Task SendToAllAsync(NotificationDto notification, CancellationToken cancellationToken);
        Task SendToAllAsync(NotificationDto notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken);
        Task SendToGroupAsync(NotificationDto notification, string group, CancellationToken cancellationToken);
        Task SendToGroupAsync(NotificationDto notification, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken);
        Task SendToGroupsAsync(NotificationDto notification, IEnumerable<string> groupNames, CancellationToken cancellationToken);
        Task SendToUserAsync(NotificationDto notification, string userId, CancellationToken cancellationToken);
        Task SendToUsersAsync(NotificationDto notification, IEnumerable<string> userIds, CancellationToken cancellationToken);

    }
}
