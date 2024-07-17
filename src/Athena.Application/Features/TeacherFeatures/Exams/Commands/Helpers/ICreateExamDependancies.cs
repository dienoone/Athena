using Athena.Application.Features.TeacherFeatures.Exams.Commands.Create;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public interface ICreateExamDependancies : ITransientService
    {
        Task<CreateExamHandlerHelper> CreateExamDependanciesAsync(CreateExamRequest request, Guid examId, Guid businessId, CancellationToken cancellationToken);
        Task<List<Guid>> CreateSectionsAsync(CreateExamRequest request, List<Guid> examGroupStudentIds, Guid examId, Guid businessId, CancellationToken cancellationToken);
        Task CreateStudentExamStatesAsync(List<Guid> sectionIds, List<Guid> studentIds, CancellationToken cancellationToken);
    }
}
