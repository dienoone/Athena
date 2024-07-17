using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Application.Features.TeacherFeatures.Groups.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Groups.Commands
{
    public record UpdateGroupScadual(Guid Id, string Day, TimeSpan StartTime, TimeSpan EndTime, bool IsDeleted);

    public class UpdateGroupRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public Guid HeadQuarterId { get; set; }
        public Guid TeacherCourseLevelYearId { get; set; }
        public int Limit { get; set; }

        public List<UpdateGroupScadual>? GroupScaduals { get; set; }
        public List<CreateGroupScadual>? NewGroupScaduals { get; set; }
    }

    public class UpdateGroupRequestHandler : IRequestHandler<UpdateGroupRequest, Guid>
    {
        private readonly ILogger<UpdateGroupRequestHandler> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IRepository<Group> _groupRepo;
        private readonly IRepository<GroupScadual> _groupScadualRepo;
        private readonly IRepository<GroupStudent> _groupStudentRepo;
        private readonly INotificationSender _notificationSender;
        private readonly INotificationService _notificationService;

        public UpdateGroupRequestHandler(
            ILogger<UpdateGroupRequestHandler> logger,
            ICurrentUser currentUser, 
            IUserService userService, 
            IReadRepository<Teacher> teacherRepo,
            IRepository<Group> groupRepo, 
            IRepository<GroupScadual> groupScadualRepo, 
            IRepository<GroupStudent> groupStudentRepo, 
            INotificationSender notificationSender, 
            INotificationService notificationService)
        {
            _logger = logger;
            _currentUser = currentUser;
            _userService = userService;
            _teacherRepo = teacherRepo;
            _groupRepo = groupRepo;
            _groupScadualRepo = groupScadualRepo;
            _groupStudentRepo = groupStudentRepo;
            _notificationSender = notificationSender;
            _notificationService = notificationService;
        }

        public async Task<Guid> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
        {
            Group? group = await _groupRepo.GetByIdAsync(request.Id, cancellationToken);
            group!.Update(request.Name, request.HeadQuarterId, null, request.Limit);
            await _groupRepo.UpdateAsync(group, cancellationToken);

            var teacher = await _teacherRepo.GetByIdAsync(_currentUser.GetBusinessId(), cancellationToken);
            var teacherUser = await _userService.GetAsync(teacher!.Id.ToString(), cancellationToken);

            if(request.GroupScaduals != null)
            {
                foreach (var groupScadual in request.GroupScaduals)
                {
                    GroupScadual? scadual = await _groupScadualRepo.GetByIdAsync(groupScadual.Id, cancellationToken);

                    if (groupScadual.IsDeleted)
                    {
                        await _groupScadualRepo.DeleteAsync(scadual!, cancellationToken);
                    }
                    else
                    {
                        scadual!.Update(groupScadual.Day, groupScadual.StartTime, groupScadual.EndTime);
                        await _groupScadualRepo.UpdateAsync(scadual!, cancellationToken);
                    }
                }
            }

            if(request.NewGroupScaduals != null)
            {
                foreach (var groupScadual in request.NewGroupScaduals)
                {
                    GroupScadual NewGroupScadual = new(groupScadual.Day, groupScadual.StartTime, groupScadual.EndTime, null, request.Id, _currentUser.GetBusinessId());
                    await _groupScadualRepo.AddAsync(NewGroupScadual, cancellationToken);
                }
            }

            List<GroupStudent>? students = await _groupStudentRepo.ListAsync(new GroupStudentsByGroupIdIncludeStudentSpec(group.Id), cancellationToken);

            CreateNotificationWrapperRequest notification = new() 
            { 
                Type = ENotificationType.UpdateGroup.ToString(),
                Label = ENotificationLabel.Information.ToString(),
                EntityId = group.Id,
                NotifierId = teacher.Id,
                BusinessId = teacher.BusinessId,
                Image = teacher.ImagePath,
                ArMessage = teacherUser!.Gender == "male" ? $"قام الاستاذ {teacherUser.FirstName + " " + teacherUser.LastName}" : $"قامت الاستاذة {teacherUser.FirstName + " " + teacherUser.LastName}",
                EnMessage = teacherUser!.Gender == "male" ? $"Mr. {teacherUser.FirstName + " " + teacherUser.LastName}" : $"Mrs. {teacherUser.FirstName + " " + teacherUser.LastName}",
            };
            var notificationDto = await _notificationService.CreateNotficationOnePlaceHolderAsync(notification, cancellationToken);
            var ids = await _notificationService.CreateNotficationRecipientsAsync(students.Select(e => e.TeacherCourseLevelYearStudent.StudentId), notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, ids, cancellationToken);

            return request.Id;
        }
    }
}
