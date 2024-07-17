using Athena.Application.Common.Interfaces;
using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec;
using Athena.Domain.Common.Const;
using Athena.Domain.Entities;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Commands
{
    // ToDo: Add TeacherCourseLevelYearId To StudentTeacherCommunication:
    // ToDo: Check if the request status is pendding.
    // ToDo: Send Notificatioin to the student.
    public record AcceptJoinRequestByRequestIdRequest(Guid Id) : IRequest<Guid>;

    public class AcceptJoinRequestByRequestIdRequestHandler : IRequestHandler<AcceptJoinRequestByRequestIdRequest, Guid>
    {
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IRepository<StudentTeacherRequest> _studentTeacherRequestRepo;
        private readonly IRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;
        private readonly IRepository<GroupStudent> _groupStudentRepo;
        private readonly INotificationService _notificationService;
        private readonly INotificationSender _notificationSender;
        private readonly IStringLocalizer<AcceptJoinRequestByRequestIdRequestHandler> _t;

        public AcceptJoinRequestByRequestIdRequestHandler(
            IReadRepository<Teacher> teacherRepo,
            IRepository<StudentTeacherRequest> studentTeacherRequestRepo,
            IRepository<GroupStudent> groupStudentRepo,
            IRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo, 
            INotificationService notificationService,
            INotificationSender notificationSender,
            IStringLocalizer<AcceptJoinRequestByRequestIdRequestHandler> t)
        {
            _teacherRepo = teacherRepo;
            _studentTeacherRequestRepo = studentTeacherRequestRepo;
            _groupStudentRepo = groupStudentRepo;
            _teacherCourseLevelYearStudentRepo = teacherCourseLevelYearStudentRepo;
            _notificationService = notificationService;
            _notificationSender = notificationSender;   
            _t = t;
        }

        public async Task<Guid> Handle(AcceptJoinRequestByRequestIdRequest request, CancellationToken cancellationToken)
        {
            var studentRequest = await _studentTeacherRequestRepo.GetBySpecAsync(new StudentTeacherRequestByIdIncludeGroupSpec(request.Id), cancellationToken);
            _ = studentRequest ?? throw new NotFoundException(_t["StudentTeacherRequest {0} Not Found!", request.Id]);

            var isStudentAssign = await _teacherCourseLevelYearStudentRepo.GetBySpecAsync(
                new TeacherCourseLevelYearStudentByStudentIdSpec(
                    studentRequest.StudentId,
                    studentRequest.BusinessId),
                cancellationToken);

            if (isStudentAssign != null)
                throw new ConflictException(_t["Student {0} is already assigned!", studentRequest.StudentId]);

            TeacherCourseLevelYearStudent newStudent = new(studentRequest.Group.TeacherCourseLevelYearId, studentRequest.StudentId, 0, studentRequest.BusinessId);
            await _teacherCourseLevelYearStudentRepo.AddAsync(newStudent, cancellationToken);

            GroupStudent groupStudent = new(studentRequest.Group.Id, newStudent.Id, studentRequest.BusinessId);
            await _groupStudentRepo.AddAsync(groupStudent, cancellationToken);

            studentRequest.Update(null, StudentTeacherRequestStatus.Accepted, null);
            await _studentTeacherRequestRepo.UpdateAsync(studentRequest, cancellationToken);
            await SendNotification(studentRequest, cancellationToken);

            return newStudent.Id;
        }

        private async Task SendNotification(StudentTeacherRequest studentRequest, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepo.GetByIdAsync(studentRequest.TeacherId, cancellationToken);

            CreateNotificationWrapperRequest notification = new()
            {
                Type = ENotificationType.AcceptRequest.ToString(),
                Label = ENotificationLabel.Success.ToString(),
                EntityId = teacher!.Id,
                NotifierId = teacher.Id,
                BusinessId = teacher.BusinessId,
                Image = teacher.ImagePath,
                ArMessage = teacher!.Gender == "male" ? $"قبل الاستاذ {teacher.Name}" : $"قبلت الاستاذه {teacher.Name}",
                EnMessage = teacher!.Gender == "male" ? $"Mr. {teacher.Name}" : $"Mrs. {teacher.Name}",
            };

            var notificationDto = await _notificationService.CreateNotficationOnePlaceHolderAsync(notification, cancellationToken);
            var ids = await _notificationService.CreateNotficationRecipientAsync(studentRequest.StudentId, notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, ids, cancellationToken);
        }
    }
}
