namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class StudentTeacherRequestByStateAndTeacherCouresLevelYearIdAndBusinessIdSpec : Specification<StudentTeacherRequest>
    {
        public StudentTeacherRequestByStateAndTeacherCouresLevelYearIdAndBusinessIdSpec(string state, Guid teacherCourseLevelYearId, Guid businessId) =>
            Query.Where(e => e.State == state && e.BusinessId == businessId && e.Group.TeacherCourseLevelYear.Id == teacherCourseLevelYearId)
            .Include(e => e.Student)
            .Include(e => e.Group);
    }
}
