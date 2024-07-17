namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class YearsWithGroupsByLevelIdAndBusinessIdSpec : Specification<TeacherCourseLevelYear>
    {
        public YearsWithGroupsByLevelIdAndBusinessIdSpec(Guid businessId, Guid levelId) =>
            Query.Where(e => e.BusinessId == businessId && e.TeacherCourseLevel.LevelId == levelId && e.Year.State)
                .Include(e => e.Year)
                .Include(e => e.Groups.Where(e => e.DeletedOn == null));
    }
}
