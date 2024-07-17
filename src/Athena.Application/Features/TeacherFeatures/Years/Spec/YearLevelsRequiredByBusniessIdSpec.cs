namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class YearLevelsRequiredByBusniessIdSpec : Specification<TeacherCourseLevelYear>
    {
        public YearLevelsRequiredByBusniessIdSpec(Guid businessId, string yearState) =>
            Query.Where(e => e.BusinessId == businessId && e.Year.State && e.Year.YearState == yearState)
               .Include(e => e.TeacherCourseLevel.Level)
               .OrderBy(e => e.TeacherCourseLevel.Level.Index);
    }
}
