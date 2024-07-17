using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Teachers.Commands
{
    // ToDo: Add Notification
    // Student Can Send a two requests for the same group?
    public record JoinTeacherRequest(Guid TeacherId, Guid GroupId) : IRequest<Guid>;

    public class JoinTeacherRequestHandler : IRequestHandler<JoinTeacherRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IRepository<StudentTeacherRequest> _studentTeacherRequestRepo;
        private readonly IRepository<StudentTeacherCommunication> _studentTeacherCommunicationRepo;
        private readonly INotificationSender _notificationSender;
        private readonly INotificationService _notificationService;

        public JoinTeacherRequestHandler(
            ICurrentUser currentUser,
            IUserService userService,
            IReadRepository<Teacher> teacherRepo,
            IReadRepository<Student> studentRepo,
            IRepository<StudentTeacherRequest> studentTeacherRequestRepo,
            IRepository<StudentTeacherCommunication> studentTeacherCommunicationRepo,
            INotificationSender notificationSender,
            INotificationService notificationService)
        {
            _currentUser = currentUser;
            _userService = userService;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _studentTeacherRequestRepo = studentTeacherRequestRepo;
            _studentTeacherCommunicationRepo = studentTeacherCommunicationRepo;
            _notificationSender = notificationSender;
            _notificationService = notificationService;
        }

        public async Task<Guid> Handle(JoinTeacherRequest request, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepo.GetByIdAsync(request.TeacherId, cancellationToken);
            var studentId = _currentUser.GetUserId();
            var businessId = teacher!.BusinessId;
            var studentUser = await _userService.GetAsync(studentId.ToString(), cancellationToken);
            var student = await _studentRepo.GetByIdAsync(studentId, cancellationToken);
            

            StudentTeacherCommunication studentTeacherCommunication = new(studentId, teacher.Id, true, businessId);

            StudentTeacherRequest studentTeacherRequest = new StudentTeacherRequest(
                studentId,
                request.TeacherId,
                request.GroupId,
                StudentTeacherRequestStatus.Pending,
                null,
                businessId
            );

            await _studentTeacherRequestRepo.AddAsync(studentTeacherRequest, cancellationToken);
            await _studentTeacherCommunicationRepo.AddAsync(studentTeacherCommunication, cancellationToken);

            #region SendNotification:

            CreateNotificationWrapperRequest notificationRequest = new()
            {
                Type = ENotificationType.JoinRequest.ToString(),
                Label = ENotificationLabel.Information.ToString(),
                EntityId = studentTeacherRequest.Id,
                NotifierId = studentId,
                BusinessId = businessId,
                Image = student!.Image,
                ArMessage = studentUser!.Gender == "male" ? $"أرسل الطالب {studentUser.FirstName + " " + studentUser.LastName}" : $"أرسلت الطالبة {studentUser.FirstName + " " + studentUser.LastName}",
                EnMessage = $"{studentUser.FirstName + " " + studentUser.LastName}",
            };
            var notificationDto = await _notificationService.CreateNotficationOnePlaceHolderAsync(notificationRequest, cancellationToken);
            var connectionIds = await _notificationService.CreateNotficationRecipientAsync(teacher.Id, notificationDto.Id, notificationRequest.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, connectionIds, cancellationToken);

            #endregion

            return studentTeacherRequest.Id;
        }
    }
}
