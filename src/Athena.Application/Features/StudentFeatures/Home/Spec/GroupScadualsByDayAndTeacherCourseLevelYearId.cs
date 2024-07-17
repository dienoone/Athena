namespace Athena.Application.Features.StudentFeatures.Home.Spec
{
    public class GroupScadualsByDayAndTeacherCourseLevelYearId : Specification<GroupScadual>
    {
        public GroupScadualsByDayAndTeacherCourseLevelYearId(string day, Guid teacherCourseLevelYearId) =>
            Query.Where(e => e.Day == day && e.Group.TeacherCourseLevelYearId == teacherCourseLevelYearId)
                .Include(e => e.Group);
    }
}
