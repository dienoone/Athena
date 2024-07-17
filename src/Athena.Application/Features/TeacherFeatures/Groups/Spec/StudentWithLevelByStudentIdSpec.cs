namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class StudentWithLevelByStudentIdSpec : Specification<Student>, ISingleResultSpecification
    {
        public StudentWithLevelByStudentIdSpec(Guid studentId) =>
            Query.Where(e => e.Id == studentId)
            .Include(e => e.LevelClassification.Level);
            
    }
}