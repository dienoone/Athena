namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec
{
    public class OpenYearIncludeLevelsByHeadQuarterIdAndStateSpec : Specification<Year>, ISingleResultSpecification
    {
        public OpenYearIncludeLevelsByHeadQuarterIdAndStateSpec(Guid headQuarterId, string state) =>
            Query.Where(y => y.YearState == state && y.State && y.TeacherCourseLevelYears.Any(ty => ty.Groups.Any(g => g.HeadQuarterId == headQuarterId)))
                .Include(e => e.TeacherCourseLevelYears.Where(e => e.Groups.Any(e => e.HeadQuarterId == headQuarterId)))
                .ThenInclude(e => e.TeacherCourseLevel.Level);
    }
}
