namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class StudentByIdSpec : Specification<Student>, ISingleResultSpecification
    {
        public StudentByIdSpec(Guid id) =>
            Query.Where(e => e.Id == id)
                .Include(e => e.LevelClassification.Level)
                .Include(e => e.LevelClassification.EducationClassification);
    }
}
