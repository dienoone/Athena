namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class GroupsForAssignStudentForTeachersByLevelIdAndBusinessIdSpec : Specification<Group>
    {
        public GroupsForAssignStudentForTeachersByLevelIdAndBusinessIdSpec(Guid businessId, Guid levelId, string yearState) =>
            Query.Where(e => e.BusinessId == businessId && e.TeacherCourseLevelYear.TeacherCourseLevel.LevelId == levelId
                    && e.TeacherCourseLevelYear.Year.State && e.TeacherCourseLevelYear.Year.YearState == yearState)
                .Include(e => e.TeacherCourseLevelYear.Year.DashboardYear);
    }
}
