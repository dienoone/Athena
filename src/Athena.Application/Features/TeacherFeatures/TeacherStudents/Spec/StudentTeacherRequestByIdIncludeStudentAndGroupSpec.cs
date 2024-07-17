namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class StudentTeacherRequestByIdIncludeStudentAndGroupSpec : Specification<StudentTeacherRequest>, ISingleResultSpecification
    {
        public StudentTeacherRequestByIdIncludeStudentAndGroupSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.Student).ThenInclude(e => e.LevelClassification.Level)
                .Include(e => e.Student).ThenInclude(e => e.LevelClassification.EducationClassification)
                .Include(e => e.Group).ThenInclude(g => g.TeacherCourseLevelYear.Year.DashboardYear);
    }
}
