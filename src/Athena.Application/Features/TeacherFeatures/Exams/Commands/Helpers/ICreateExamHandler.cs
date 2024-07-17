namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public interface ICreateExamHandler : ITransientService
    {
        Task ActivateExamJob(Teacher teacher, Guid examId, List<Guid> studentIds, CancellationToken cancellationToken);
        Task EndActiveExamJob(Teacher teacher, Guid examId, CancellationToken cancellationToken);
        Task CorrectExamJob(Teacher teacher, Guid examId, CancellationToken cancellationToken);


        Task CreateExamNotifications(Teacher teacher, Exam newExam, List<Guid> studentIds, CancellationToken cancellationToken);
    }
}
