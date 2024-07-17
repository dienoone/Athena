namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class TeacherYearsByLevelClassificationIdAndYearStateSpec : Specification<TeacherCourseLevelYear>, ISingleResultSpecification
    {
        public TeacherYearsByLevelClassificationIdAndYearStateSpec(Guid levelId, string state, Guid businessId) =>
            Query.Where(e => e.Year.YearState == state && e.Year.State && e.BusinessId == businessId
                && e.TeacherCourseLevel.LevelId == levelId)
            .Include(e => e.Year)
            .ThenInclude(e => e.DashboardYear);
    }
}
