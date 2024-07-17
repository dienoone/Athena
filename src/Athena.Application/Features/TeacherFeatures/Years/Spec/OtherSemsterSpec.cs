namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class OtherSemsterSpec : Specification<TeacherCourseLevelYearSemster>, ISingleResultSpecification
    {
        public OtherSemsterSpec(Guid teacherCourseLevelYearId, string semster) =>
            Query.Where(e => e.TeacherCourseLevelYearId == teacherCourseLevelYearId && e.Semster.Equals(semster));
    }
}
