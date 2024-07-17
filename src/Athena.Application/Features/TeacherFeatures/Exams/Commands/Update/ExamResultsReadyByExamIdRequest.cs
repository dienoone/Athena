using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Update
{
    // ToDo: Check if all students is corrected.
    public record ExamResultsReadyByExamIdRequest(Guid ExamId) : IRequest<Guid>;

    public class ExamResultsReadyByExamIdRequestHandler : IRequestHandler<ExamResultsReadyByExamIdRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IRepository<Exam> _examRepo;
        private readonly IRepository<ExamGroup> _examGroupRepo;
        private readonly INotificationService _notificationService;
        private readonly INotificationSender _notificationSender;
        private readonly IStringLocalizer<ExamResultsReadyByExamIdRequestHandler> _t;

        public ExamResultsReadyByExamIdRequestHandler(
            ICurrentUser currentUser, 
            IReadRepository<Teacher> teacherRepo, 
            IReadRepository<ExamGroupStudent> examGroupStudentRepo, 
            IRepository<Exam> examRepo, 
            IRepository<ExamGroup> examGroupRepo, 
            INotificationService notificationService, 
            INotificationSender notificationSender, 
            IStringLocalizer<ExamResultsReadyByExamIdRequestHandler> t)
        {
            _currentUser = currentUser;
            _teacherRepo = teacherRepo;
            _examGroupStudentRepo = examGroupStudentRepo;
            _examRepo = examRepo;
            _examGroupRepo = examGroupRepo;
            _notificationService = notificationService;
            _notificationSender = notificationSender;
            _t = t;
        }

        public async Task<Guid> Handle(ExamResultsReadyByExamIdRequest request, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetByIdAsync(request.ExamId, cancellationToken);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", request.ExamId]);

            var examGroups = await _examGroupRepo.ListAsync(new ExamGroupsByExamIdSpec(request.ExamId), cancellationToken);
            if (examGroups.Any())
            {
                foreach (var group in examGroups)
                {
                    group.Update(null, true);
                    await _examGroupRepo.UpdateAsync(group, cancellationToken);
                }
            }

            exam.Update(null, null, ExamState.Finished, null, null, null, null, null, null, true, null);
            await _examRepo.UpdateAsync(exam, cancellationToken);

            await SendNotifications(exam, cancellationToken);

            return exam.Id;
        }

        private async Task SendNotifications(Exam? exam, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepo.GetByIdAsync(_currentUser.GetBusinessId(), cancellationToken);

            CreateNotificationWrapperRequest notification = new()
            {
                Type = ENotificationType.ExamResult.ToString(),
                Label = ENotificationLabel.Information.ToString(),
                EntityId = exam!.Id,
                NotifierId = teacher!.Id,
                BusinessId = teacher.BusinessId,
                Image = teacher.ImagePath,
                ArMessage = $"{exam.Name}",
                EnMessage = $"{exam.Name}",
            };

            var examGroupStudents = await _examGroupStudentRepo.ListAsync(new ExamGroupStudentByExamIdIncludeStudentSpec(exam.Id), cancellationToken);
            var notificationDto = await _notificationService.CreateNotficationOnePlaceHolderAsync(notification, cancellationToken);
            var ids = await _notificationService.CreateNotficationRecipientsAsync(examGroupStudents.Select(e => e.GroupStudent.TeacherCourseLevelYearStudent.StudentId), notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, ids, cancellationToken);
        }
    }
}
