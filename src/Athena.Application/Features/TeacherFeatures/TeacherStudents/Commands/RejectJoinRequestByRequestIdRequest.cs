using Athena.Application.Common.Interfaces;
using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Commands
{
    // ToDo: Add Notification Message only
    // ToDo: Check if this is the third request then change the communication to false
    public record RejectJoinRequestByRequestIdRequest(Guid Id, string? Message) : IRequest<Guid>;
    public class RejectJoinRequestByRequestIdRequestHandler : IRequestHandler<RejectJoinRequestByRequestIdRequest, Guid>
    {
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IRepository<StudentTeacherRequest> _studentTeacherRequestRepo;
        private readonly INotificationService _notificationService;
        private readonly INotificationSender _notificationSender;
        private readonly IStringLocalizer<RejectJoinRequestByRequestIdRequest> _t;

        public RejectJoinRequestByRequestIdRequestHandler(
            IReadRepository<Teacher> teacherRepo, 
            IRepository<StudentTeacherRequest> studentTeacherRequestRepo, 
            INotificationService notificationService, 
            INotificationSender notificationSender, 
            IStringLocalizer<RejectJoinRequestByRequestIdRequest> t)
        {
            _teacherRepo = teacherRepo;
            _studentTeacherRequestRepo = studentTeacherRequestRepo;
            _notificationService = notificationService;
            _notificationSender = notificationSender;
            _t = t;
        }

        public async Task<Guid> Handle(RejectJoinRequestByRequestIdRequest request, CancellationToken cancellationToken)
        {
            var studentRequest = await _studentTeacherRequestRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = studentRequest ?? throw new NotFoundException(_t["StudentTeacherRequest {0} Not Found!", request.Id]);

            studentRequest.Update(null, StudentTeacherRequestStatus.Rejected, request.Message);
            await _studentTeacherRequestRepo.UpdateAsync(studentRequest, cancellationToken);
            
            await SendNotification(studentRequest, cancellationToken);

            return request.Id;
        }

        private async Task SendNotification(StudentTeacherRequest studentRequest, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepo.GetByIdAsync(studentRequest.TeacherId, cancellationToken);

            CreateNotificationWrapperRequest notification = new()
            {
                Type = ENotificationType.RejectRequest.ToString(),
                Label = ENotificationLabel.Error.ToString(),
                EntityId = teacher!.Id,
                NotifierId = teacher.Id,
                BusinessId = teacher.BusinessId,
                Image = teacher.ImagePath,
                ArMessage = teacher!.Gender == "male" ? $"رفض الاستاذ {teacher.Name}" : $"رفضت الاستاذه {teacher.Name}",
                EnMessage = teacher!.Gender == "male" ? $"Mr. {teacher.Name}" : $"Mrs. {teacher.Name}",
            };

            var notificationDto = await _notificationService.CreateNotficationOnePlaceHolderAsync(notification, cancellationToken);
            var ids = await _notificationService.CreateNotficationRecipientAsync(studentRequest.StudentId, notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, ids, cancellationToken);
        }
    }
}
