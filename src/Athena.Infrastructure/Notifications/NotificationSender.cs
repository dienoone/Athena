using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Microsoft.AspNetCore.SignalR;
using static Athena.Shared.Notifications.NotificationConstants;


namespace Athena.Infrastructure.Notifications
{
    public class NotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public NotificationSender(
            IHubContext<NotificationHub> notificationHubContext) =>
            (_notificationHubContext) = (notificationHubContext);

        public Task BroadcastAsync(NotificationDto notification, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.All
                .SendAsync(NotificationFromServer, notification, cancellationToken);

        public Task BroadcastAsync(NotificationDto notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.AllExcept(excludedConnectionIds)
                .SendAsync(NotificationFromServer, notification, cancellationToken);

        public Task SendToAllAsync(NotificationDto notification, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.Group($"GroupTenant-{/*_currentTenant.Id*/""}")
                .SendAsync(NotificationFromServer, notification, cancellationToken);

        public Task SendToAllAsync(NotificationDto notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.GroupExcept($"GroupTenant-{/*_currentTenant.Id*/""}", excludedConnectionIds)
                .SendAsync(NotificationFromServer, notification, cancellationToken);

        public Task SendToGroupAsync(NotificationDto notification, string group, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.Group(group)
                .SendAsync(NotificationFromServer, notification, cancellationToken);

        public Task SendToGroupAsync(NotificationDto notification, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.GroupExcept(group, excludedConnectionIds)
                .SendAsync(NotificationFromServer, notification, cancellationToken);

        public Task SendToGroupsAsync(NotificationDto notification, IEnumerable<string> groupNames, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.Groups(groupNames)
                .SendAsync(NotificationFromServer, notification, cancellationToken);

        public Task SendToUserAsync(NotificationDto notification, string userId, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.Client(userId)
                .SendAsync(NotificationFromServer, notification, cancellationToken);

        public Task SendToUsersAsync(NotificationDto notification, IEnumerable<string> userIds, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.Clients(userIds)
                .SendAsync(NotificationFromServer, notification, cancellationToken);

    }
}
