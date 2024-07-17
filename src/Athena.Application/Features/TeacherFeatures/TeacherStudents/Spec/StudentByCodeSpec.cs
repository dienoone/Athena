namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class StudentByCodeSpec : Specification<Student>, ISingleResultSpecification
    {
        public StudentByCodeSpec(string code) =>
            Query.Where(e => e.Code == code)
                .Include(e => e.LevelClassification.Level)
                .Include(e => e.LevelClassification.EducationClassification);
    }
}
