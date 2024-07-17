using Athena.Application.Features.DashboardFeatures.Notifications.Dtos;
using Athena.Application.Features.DashboardFeatures.Notifications.Spec;

namespace Athena.Application.Features.DashboardFeatures.Notifications.Queries
{
    public record GetNotificationTypesListRequest() : IRequest<List<NotificationTypeDto>>;

    public class GetNotificationTypesListRequestHandler : IRequestHandler<GetNotificationTypesListRequest, List<NotificationTypeDto>>
    {
        private readonly IReadRepository<NotificationType> _notificationTypeRepo;

        public GetNotificationTypesListRequestHandler(IReadRepository<NotificationType> notificationTypeRepo)
        {
            _notificationTypeRepo = notificationTypeRepo;
        }

        public async Task<List<NotificationTypeDto>> Handle(GetNotificationTypesListRequest request, CancellationToken cancellationToken)
        {
            var notificationTypes = await _notificationTypeRepo.ListAsync(new NotificationTypesIncludeTemplatesSpec(), cancellationToken);
            return notificationTypes.Adapt<List<NotificationTypeDto>>();
        }
    }
}
