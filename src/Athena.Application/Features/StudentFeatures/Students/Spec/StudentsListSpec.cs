namespace Athena.Application.Features.StudentFeatures.Students.Spec
{
    public class StudentsListSpec : Specification<Student>
    {
        public StudentsListSpec() =>
            Query.Include(e => e.LevelClassification.EducationClassification)
                .Include(e => e.LevelClassification.Level);
        
    }
}
