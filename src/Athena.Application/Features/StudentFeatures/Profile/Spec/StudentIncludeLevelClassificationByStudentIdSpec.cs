namespace Athena.Application.Features.StudentFeatures.Profile.Spec
{
    public class StudentIncludeLevelClassificationByStudentIdSpec : Specification<Student>, ISingleResultSpecification
    {
        public StudentIncludeLevelClassificationByStudentIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.LevelClassification.Level)
                .Include(e => e.LevelClassification.EducationClassification);
    }
}
