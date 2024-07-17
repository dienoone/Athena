using Athena.Application.Features.DashboardFeatures.Courses.Commands;
using Athena.Application.Features.DashboardFeatures.Notifications.Commands;
using Athena.Application.Features.DashboardFeatures.Notifications.Dtos;
using Athena.Application.Features.DashboardFeatures.Notifications.Queries;
using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.NotificatioinFeatures.Queries;
using Athena.Domain.Entities;

namespace Athena.Api.Controllers.Dashboard
{
    public class NotificationBaseController : VersionNeutralApiDashboardGroupController
    {
        [HttpGet]
        [AllowAnonymous]
        [OpenApiOperation("Get notification types list.", "")]
        public async Task<List<NotificationTypeDto>> GetAsync()
        {
            return await Mediator.Send(new GetNotificationTypesListRequest());
        }

        //GetNotificationsFormDataBaseRequest
        [HttpGet("all")]
        [AllowAnonymous]
        [OpenApiOperation("Get notification types list.", "")]
        public async Task<List<Notification>> GetAllAsync()
        {
            return await Mediator.Send(new GetNotificationsFormDataBaseRequest());
        }

        //GetSignalRConnectionsRequest
        [HttpGet("signalR")]
        [AllowAnonymous]
        [OpenApiOperation("Get notification types list.", "")]
        public async Task<List<SignalRConnectionDto>> GetSignalRConnectionsAsync()
        {
            return await Mediator.Send(new GetSignalRConnectionsRequest());
        }

        [HttpPost]
        [AllowAnonymous]
        [OpenApiOperation("Create a new notification type.", "")]
        public async Task<Guid> CreateAsync(CreateNotificationTypeRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost("template")]
        [AllowAnonymous]
        [OpenApiOperation("Create a new notification type template.", "")]
        public async Task<Guid> CreateTemplateAsync(CreateNotificationTypeTemplateRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Update a notification type.", "")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateNotificationTypeByIdRequest request, Guid id)
        {
            return id != request.Id
           ? BadRequest()
           : Ok(await Mediator.Send(request));
        }

        [HttpPut("template/{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Update a notification type template.", "")]
        public async Task<ActionResult<Guid>> UpdateTemplateAsync(UpdateNotificationTypeTemplateByIdRequest request, Guid id)
        {
            return id != request.Id
           ? BadRequest()
           : Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Delete a notification type.", "")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await Mediator.Send(new DeleteCourseRequest(id));
        }

        [HttpDelete("template/{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Delete a notification type template.", "")]
        public async Task<Guid> DeleteTemplateAsync(Guid id)
        {
            return await Mediator.Send(new DeleteNotificationTypeTemplateByIdRequest(id));
        }
    }
}
