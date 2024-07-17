namespace Athena.Application.Features.TeacherFeatures.Home.Spec
{
    public class GroupScadualsIncludeGroupByDayAndBusinessIdSpec : Specification<GroupScadual>
    {
        public GroupScadualsIncludeGroupByDayAndBusinessIdSpec(string day, Guid businessId) =>
            Query.Where(e => e.Day == day && e.BusinessId == businessId && e.Group.TeacherCourseLevelYear.Year.State)
                .Include(e => e.Group).ThenInclude(e => e.HeadQuarter);
    }
}
