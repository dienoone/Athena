namespace Athena.Application.Features.StudentFeatures.Home.Spec
{
    public class GroupStudentByTeacherCourseLevelYearStudentIdSpec : Specification<GroupStudent>, ISingleResultSpecification
    {
        public GroupStudentByTeacherCourseLevelYearStudentIdSpec(Guid teacherCourseLevelYearStudentId, string day) =>
            Query.Where(e => e.TeacherCourseLevelYearStudentId == teacherCourseLevelYearStudentId && e.Group.GroupScaduals.Any(e => e.Day == day))
            .Include(e => e.Group)
            .ThenInclude(e => e.GroupScaduals);
    }
}
