namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class TeacherCourseLevelYearByYearIdAndTeacherCourseLevelIdSpec : Specification<TeacherCourseLevelYear>, ISingleResultSpecification
    {
        public TeacherCourseLevelYearByYearIdAndTeacherCourseLevelIdSpec(Guid yearId, Guid teacherCouserLevelId) =>
            Query.Where(e => e.YearId == yearId && e.TeacherCourseLevelId == teacherCouserLevelId);
    }
}
