namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class YearsForExploreTeacherByBusinessIdAndStateIncludeTeacherCourseLevelYearsSpec : Specification<Year>, ISingleResultSpecification
    {
        public YearsForExploreTeacherByBusinessIdAndStateIncludeTeacherCourseLevelYearsSpec(string state, Guid businessId) =>
            Query.Where(e => e.BusinessId == businessId && e.State && e.DashboardYear.State == state)
                .Include(e => e.DashboardYear)
                .Include(e => e.TeacherCourseLevelYears.Where(e => e.DeletedOn == null))
                .ThenInclude(e => e.TeacherCourseLevel.Level);
    }
}
