namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class GroupStudentsListByTeacherCourseLevelYearIdSpec : Specification<GroupStudent>
    {
        public GroupStudentsListByTeacherCourseLevelYearIdSpec(Guid teacherCouseLevelYearId, Guid teacherCourseLevelId) =>
            Query.Where(e => e.TeacherCourseLevelYearStudent.TeacherCourseLevelYearId == teacherCouseLevelYearId
                && e.TeacherCourseLevelYearStudent.TeacherCourseLevelYear.TeacherCourseLevelId == teacherCourseLevelId)
            .Include(e => e.Group)
            .Include(e => e.TeacherCourseLevelYearStudent.Student);
    }
}
