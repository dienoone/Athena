using Athena.Application.Common.Exceptions;
using Athena.Application.Features.NotificatioinFeatures.Commands;
using Athena.Application.Features.NotificatioinFeatures.Queries;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;
using Athena.Infrastructure.Notifications.Spec;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Athena.Infrastructure.Notifications
{
    [Authorize]
    public class NotificationHub : Hub, ITransientService
    {
        private readonly ILogger<NotificationHub> _logger;
        private readonly IUserService _userService;
        private readonly ISignalRConnectionService _signalRConnectionService;
        private readonly IReadRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<NotificationHub> _t;

        public NotificationHub(
            ILogger<NotificationHub> logger, 
            IUserService userService,
            ISignalRConnectionService signalRConnectionService,
            IReadRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo,
            IMediator mediator,
            IStringLocalizer<NotificationHub> t)
        {
            _logger = logger;
            _userService = userService;
            _signalRConnectionService = signalRConnectionService;
            _teacherCourseLevelYearStudentRepo = teacherCourseLevelYearStudentRepo;
            _mediator = mediator;
            _t = t;
        }

        #region HandleConnections:

        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            _ = user ?? throw new UnauthorizedException("Authentication Failed.");

            var userId = user.GetUserId();

            if (!string.IsNullOrEmpty(userId))
            {
                var roles = await _userService.GetRolesAsync(userId, new());

                foreach (var role in roles)
                {
                    if (role == "Teacher")
                    {
                        var connection = await _signalRConnectionService.CreateConnection(Guid.Parse(userId), Context.ConnectionId, EHubTypes.Notification.ToString(), Guid.Parse(user.GetBusinessId()!));
                        await AddToGroups(connection.Id, $"GroupTeacher-{user!.GetBusinessId()}", NotificationGroups.TeacherGroup);
                    }
                    else if (role == "Student")
                    {
                        var connection = await _signalRConnectionService.CreateConnection(Guid.Parse(userId), Context.ConnectionId, EHubTypes.Notification.ToString(), null);
                        await AddToGroups(connection.Id, NotificationGroups.StudentGroup);
                        await AddToTeacherGroups(Guid.Parse(userId), connection.Id);
                    }
                }
            }

            await base.OnConnectedAsync();

            _logger.LogInformation("A client connected to NotificationHub: {connectionId}", Context.ConnectionId);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            // NotificationGroups
            var user = Context.User;
            _ = user ?? throw new UnauthorizedException("Authentication Failed.");

            var userId = Guid.Parse(user.GetUserId()!);

            if (!string.IsNullOrEmpty(userId.ToString()))
            {
                var roles = await _userService.GetRolesAsync(userId.ToString(), new());
                foreach (var role in roles)
                {
                    if (role == "Teacher")
                    {
                        await RemoveFromGroups($"GroupTeacher-{Guid.Parse(user.GetBusinessId()!)}", NotificationGroups.TeacherGroup);
                    }
                    else if (role == "Student")
                    {
                        await RemoveFromGroups(NotificationGroups.StudentGroup);
                        await RemoveFromTeacherGroups(userId);
                    }
                }
            }

            await _signalRConnectionService.DeleteConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);

            _logger.LogInformation("A client disconnected from NotificationHub: {connectionId}", Context.ConnectionId);
        }

        #endregion

        public async Task GetNotifications()
        {
            var userId = Context.User!.GetUserId();
            var notifications = await _mediator.Send(new GetLastNotificationsRequest(Guid.Parse(userId!)));
            await Clients.Caller.SendAsync("Notifications", notifications);
        }

        public async Task ChangeStatus(Guid id)
        {
            var userId = Context.User!.GetUserId();
            var notification = await _mediator.Send(new ChangeStatusByNotificationIdRequest(id, Guid.Parse(userId!)));
            var notifications = await _mediator.Send(new GetLastNotificationsRequest(Guid.Parse(userId!)));
            await Clients.Caller.SendAsync("Notifications", notifications);
        }

        #region Helpers:

        private async Task AddToGroups(Guid connectionId, params string[] groupNames)
        {
            foreach (var groupName in groupNames)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await _signalRConnectionService.CreateConnectionGroup(connectionId, groupName);
            }
        }
        private async Task AddToTeacherGroups(Guid userId, Guid connectionId)
        {
            var teacherCourseLevelYearStudents = await _teacherCourseLevelYearStudentRepo.ListAsync(new TeacherCourseLevelYearStudentByStudentIdSpec(userId));

            foreach (var student in teacherCourseLevelYearStudents)
            {
                var groupName = $"GroupTeacher-{student.BusinessId}";
                await AddToGroups(connectionId, groupName);
            }
        }

        private async Task RemoveFromTeacherGroups(Guid userId)
        {
            var teacherCourseLevelYearStudents = await _teacherCourseLevelYearStudentRepo.ListAsync(new TeacherCourseLevelYearStudentByStudentIdSpec(userId));

            foreach (var student in teacherCourseLevelYearStudents)
            {
                var groupName = $"GroupTeacher-{student.BusinessId}";
                await RemoveFromGroups(groupName);
            }
        }
        private async Task RemoveFromGroups(params string[] groupNames)
        {
            foreach (var groupName in groupNames)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }
        }

        #endregion

    }
}
