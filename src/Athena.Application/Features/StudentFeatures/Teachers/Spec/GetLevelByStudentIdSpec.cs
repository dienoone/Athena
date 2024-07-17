namespace Athena.Application.Features.StudentFeatures.Teachers.Spec;

public sealed class GetLevelByStudentIdSpec : Specification<Student>, ISingleResultSpecification
{
    public GetLevelByStudentIdSpec(Guid studentId) =>
        Query.Where(e => e.Id == studentId)
            .Include(e => e.LevelClassification.Level);
}