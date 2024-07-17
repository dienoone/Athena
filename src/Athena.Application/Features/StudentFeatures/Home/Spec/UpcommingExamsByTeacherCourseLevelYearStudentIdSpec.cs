namespace Athena.Application.Features.StudentFeatures.Home.Spec
{
    public class UpcommingExamsByTeacherCourseLevelYearStudentIdSpec : Specification<Exam>
    {
        public UpcommingExamsByTeacherCourseLevelYearStudentIdSpec(Guid teacherCourseLevelYearStudentId, string[] states) =>
            Query.Where(e => e.TeacherCourseLevelYear.TeacherCourseLevelYearStudents
            .Any(tclyStudent => tclyStudent.Id == teacherCourseLevelYearStudentId) && states.Contains(e.State));
    }
}
