using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.TeacherFeatures.Exams.Commands.Create;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public class CreateExamHandler : ICreateExamHandler
    {
        private readonly IRepository<Exam> _examRepo;
        private readonly IRepository<StudentSectionState> _studentSectionStateRepo;
        private readonly IRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IRepository<ExamStudentAnswer> _examStudentAnswerRepo;
        private readonly INotificationSender _notificationSender;
        private readonly INotificationService _notificationService;
        private readonly ITakeExamService _takeExamService;
        private readonly ISignalRConnectionService _connectionService;

        public CreateExamHandler(
            IRepository<Exam> examRepo, 
            IRepository<StudentSectionState> studentSectionStateRepo, 
            IRepository<ExamGroupStudent> examGroupStudentRepo, 
            IRepository<ExamStudentAnswer> examStudentAnswerRepo, 
            INotificationSender notificationSender, 
            INotificationService notificationService, 
            ITakeExamService takeExamService, 
            ISignalRConnectionService connectionService)
        {
            _examRepo = examRepo;
            _studentSectionStateRepo = studentSectionStateRepo;
            _examGroupStudentRepo = examGroupStudentRepo;
            _examStudentAnswerRepo = examStudentAnswerRepo;
            _notificationSender = notificationSender;
            _notificationService = notificationService;
            _takeExamService = takeExamService;
            _connectionService = connectionService;
        }

        public async Task ActivateExamJob(Teacher teacher, Guid examId, List<Guid> studentIds, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetByIdAsync(examId, cancellationToken);

            if (exam != null)
            {
                exam.Update(null, null, ExamState.Active, null, null, null, null, null, null, null, null);
                await _examRepo.UpdateAsync(exam, cancellationToken);

                await ActiveExamNotifications(teacher, examId, studentIds, cancellationToken);
            }
        }

        public async Task CorrectExamJob(Teacher teacher, Guid examId, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetByIdAsync(examId, cancellationToken);
            if (exam != null)
            {
                var examGroupStudents = await _examGroupStudentRepo.ListAsync(new ExamGroupStudentsIncludeExamStudentAnswersByExamIdSpec(examId), cancellationToken);
                foreach (var examGroupStudent in examGroupStudents)
                {
                    if (examGroupStudent.State)
                    {
                        var degree = examGroupStudent.ExamStudentAnswers.Sum(e => e.Degree);
                        var percentage = (degree / exam.FinalDegree) * 100;
                        var state = GetStudentState(percentage);
                        var points = CalculateExamPoints(percentage, examGroupStudent.State);

                        examGroupStudent.Update(state, null, degree, points);
                        await _examGroupStudentRepo.UpdateAsync(examGroupStudent, cancellationToken);

                        var answers = examGroupStudent.ExamStudentAnswers;
                        foreach (var answer in answers)
                        {
                            if(answer.Question.Type == QuestionTypes.MCQ)
                            {
                                answer.Update(null, null, null, true, null);
                                await _examStudentAnswerRepo.UpdateAsync(answer, cancellationToken);
                            }
                            else
                            {
                                if (!answer.IsAnswered)
                                {
                                    answer.Update(null, null, 0, true, null);
                                    await _examStudentAnswerRepo.UpdateAsync(answer, cancellationToken);
                                }
                            }
                                
                                
                        }
                    }
                    else
                    {
                        var answers = examGroupStudent.ExamStudentAnswers;
                        foreach (var answer in answers)
                        {
                            answer.Update(null, null, null, true, null);
                            await _examStudentAnswerRepo.UpdateAsync(answer, cancellationToken);
                        }

                        examGroupStudent.Update(null, null, null, -12);
                        await _examGroupStudentRepo.UpdateAsync(examGroupStudent, cancellationToken);
                    }
                }

                await CorrectExamNotifications(teacher, examId, exam.Name, cancellationToken);
            }
        }

        public async Task EndActiveExamJob(Teacher teacher, Guid examId, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetByIdAsync(examId, cancellationToken);

            if (exam != null)
            {
                exam.Update(null, null, ExamState.Correcting, null, null, null, null, null, null, null, null);
                await _examRepo.UpdateAsync(exam, cancellationToken);

                // remove students from exam hub:
                await _takeExamService.NotifyEndExamAsync($"{EHubTypes.TakeExam}-{examId}", cancellationToken);
                var signalRConnections = await _connectionService.GetConnectionsForGroup($"{EHubTypes.TakeExam}-{examId}", cancellationToken);
                foreach (var connection in signalRConnections)
                {
                    await _connectionService.DeleteConnection(connection.ConnectionId);
                    await _takeExamService.DeleteFromGroupAsync($"{EHubTypes.TakeExam}-{examId}", connection.ConnectionId, cancellationToken);
                }

                // Remove StudentSectionStates:
                var studentSectionStates = await _studentSectionStateRepo.ListAsync(new StudentSectionStatesByExamIdSpec(examId), cancellationToken);
                await _studentSectionStateRepo.DeleteRangeAsync(studentSectionStates, cancellationToken);
            }
        }

        private static string GetStudentState(double percentage)
        {
            if (percentage >= 95)
                return ExamStudentState.Excellent;
            else if (percentage >= 50 && percentage < 95)
                return ExamStudentState.Successful;
            else
                return ExamStudentState.Failure;
        }

        private static int CalculateExamPoints(double percentage, bool attendance)
        {
            if (attendance)
            {
                if (percentage > 95)
                {
                    return 10;
                }
                else if (percentage >= 50)
                {
                    return 7;
                }
                else
                {
                    return -10;
                }
            }
            else
            {
                return -12;
            }

        }

        #region Notifications:

        public async Task CreateExamNotifications(Teacher teacher, Exam newExam, List<Guid> studentIds, CancellationToken cancellationToken)
        {
            CreateNotificationWrapperRequest notification = new()
            {
                Type = ENotificationType.UpcommingExam.ToString(),
                Label = ENotificationLabel.Information.ToString(),
                EntityId = newExam.Id,
                NotifierId = teacher!.Id,
                BusinessId = teacher.BusinessId,
                Image = teacher.ImagePath,
                ArMessage = teacher!.Gender == "male" ? $"أعد الأستاذ {teacher.Name}" : $"أعدت الاستاذة {teacher.Name}",
                EnMessage = teacher!.Gender == "male" ? $"Mr. {teacher.Name}" : $"Mrs. {teacher.Name}",
            };
            var notificationDto = await _notificationService.CreateNotficationOnePlaceHolderAsync(notification, cancellationToken);
            var connectionIds = await _notificationService.CreateNotficationRecipientsAsync(studentIds, notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, connectionIds, cancellationToken);
        }

        private async Task ActiveExamNotifications(Teacher teacher, Guid examId, List<Guid> studentIds, CancellationToken cancellationToken)
        {
            CreateNotificationWrapperRequest notification = new()
            {
                Type = ENotificationType.ActiveExam.ToString(),
                Label = ENotificationLabel.Success.ToString(),
                EntityId = examId,
                NotifierId = teacher!.Id,
                BusinessId = teacher.BusinessId,
                Image = teacher.ImagePath
            };

            var notificationDto = await _notificationService.CreateNotficationOnePlaceHolderAsync(notification, cancellationToken);
            var connectionIds = await _notificationService.CreateNotficationRecipientsAsync(studentIds, notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, connectionIds, cancellationToken);
        }

        private async Task CorrectExamNotifications(Teacher teacher, Guid examId, string examName, CancellationToken cancellationToken)
        {
            CreateNotificationWrapperRequest notification = new()
            {
                Type = ENotificationType.CorrectExam.ToString(),
                Label = ENotificationLabel.Information.ToString(),
                EntityId = examId,
                NotifierId = teacher!.Id,
                BusinessId = teacher.BusinessId,
                ArMessage = $"امتحان \"{examName}\"",
                EnMessage = $"\"{examName}\" Exam",
            };
            var notificationDto = await _notificationService.CreateNotficationOnePlaceHolderAsync(notification, cancellationToken);
            var connectionIds = await _notificationService.CreateNotficationRecipientAsync(teacher.Id, notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, connectionIds, cancellationToken);
        }

        //ToDo: Do This Template:
        private async Task FinishExamNotifications(Teacher teacher, Exam newExam, List<Guid> studentIds, CancellationToken cancellationToken)
        {
            CreateNotificationWrapperRequest notification = new()
            {
                Type = ENotificationType.UpcommingExam.ToString(),
                Label = ENotificationLabel.Information.ToString(),
                EntityId = newExam.Id,
                NotifierId = teacher!.Id,
                BusinessId = teacher.BusinessId,
                Image = teacher.ImagePath,
                ArMessage = teacher!.Gender == "male" ? $"أعد الأستاذ {teacher.Name}" : $"أعدت الاستاذة {teacher.Name}",
                EnMessage = teacher!.Gender == "male" ? $"Mr. {teacher.Name}" : $"Mrs. {teacher.Name}",
            };
            var notificationDto = await _notificationService.CreateNotficationOnePlaceHolderAsync(notification, cancellationToken);
            var connectionIds = await _notificationService.CreateNotficationRecipientsAsync(studentIds, notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, connectionIds, cancellationToken);
        }

        #endregion

    }
}
