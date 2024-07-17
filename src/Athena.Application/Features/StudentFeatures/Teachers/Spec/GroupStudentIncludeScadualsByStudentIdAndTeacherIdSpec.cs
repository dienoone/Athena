namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class GroupStudentIncludeScadualsByStudentIdAndTeacherIdSpec : Specification<GroupStudent>, ISingleResultSpecification
    {
        public GroupStudentIncludeScadualsByStudentIdAndTeacherIdSpec(Guid teacherId, Guid studentId) =>
            Query.Where(e => e.TeacherCourseLevelYearStudent.StudentId == studentId
                    && e.Group.TeacherCourseLevelYear.TeacherCourseLevel.TeacherId == teacherId)
                 .Include(e => e.Group)
                 .ThenInclude(e => e.GroupScaduals)
                 .Where(e => e.Group.GroupScaduals.Any(gs => gs.DeletedOn == null));
    }
}
