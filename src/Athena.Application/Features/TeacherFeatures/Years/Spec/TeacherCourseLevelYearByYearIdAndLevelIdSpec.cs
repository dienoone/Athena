namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class TeacherCourseLevelYearByYearIdAndLevelIdSpec : Specification<TeacherCourseLevelYear>, ISingleResultSpecification
    {
        public TeacherCourseLevelYearByYearIdAndLevelIdSpec(Guid id, Guid levelId) =>
            Query
                .Where(e => e.TeacherCourseLevelId == levelId && e.YearId == id);
    }
}
