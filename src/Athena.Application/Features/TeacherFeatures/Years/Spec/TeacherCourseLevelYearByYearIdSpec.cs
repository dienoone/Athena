namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class TeacherCourseLevelYearByYearIdSpec : Specification<TeacherCourseLevelYear>
    {
        public TeacherCourseLevelYearByYearIdSpec(Guid yearId) =>
            Query.Where(e => e.YearId == yearId);
    }
}
