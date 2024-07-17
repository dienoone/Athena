using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.NotificatioinFeatures.Queries;
using Athena.Application.Features.TeacherFeatures.Notifications.Commands;

namespace Athena.Api.Controllers.Teacher
{
    public class NotificationsController : VersionNeutralApiTeacherGroupController
    {
        [HttpGet]
        [OpenApiOperation("Get all-notifications.", "")]
        public async Task<List<NotificationDto>> GetAllNotificationsAsync()
        {
            return await Mediator.Send(new GetAllNotificationsRequest());
        }

        [HttpPut("{id:guid}")]
        [OpenApiOperation("update notification-status-by-notification-id.", "")]
        public async Task<Guid> UpdateNotificationStatusAsync(Guid id)
        {
            return await Mediator.Send(new ChangeNotificationStatusByIdRequest(id));
        }

        [HttpDelete]
        [OpenApiOperation("delete selected-notifications.", "")]
        public async Task<Guid> DeleteSectionAsync(DeleteNotificationRecipientsByIdsRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
