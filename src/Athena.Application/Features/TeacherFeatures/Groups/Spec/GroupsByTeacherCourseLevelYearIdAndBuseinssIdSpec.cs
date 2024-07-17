namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class GroupsForUpdateStudentInfoByTeacherCourseLevelYearIdAndBuseinssIdSpec : Specification<Group>
    {
        public GroupsForUpdateStudentInfoByTeacherCourseLevelYearIdAndBuseinssIdSpec(Guid teacherCourseLevelYearId,string yearState, Guid businessId) =>
            Query.Where(e => e.TeacherCourseLevelYearId == teacherCourseLevelYearId && e.TeacherCourseLevelYear.Year.State
                && e.TeacherCourseLevelYear.Year.YearState == yearState && e.BusinessId == businessId);
    }
}
