namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Create
{
    public interface ITakeExamService : ITransientService
    {
        Task DeleteFromGroupAsync(string group, string connectionId, CancellationToken cancellationToken);
        Task NotifyEndExamAsync(string group, CancellationToken cancellationToken);
    }
}
