namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class TeacherCourseLevelYearSemsterByYearLevelIdAndSemsterNameSpec : Specification<TeacherCourseLevelYearSemster>, ISingleResultSpecification
    {
        public TeacherCourseLevelYearSemsterByYearLevelIdAndSemsterNameSpec(Guid yearLevelId, string Semster) =>
            Query
                .Where(e => e.TeacherCourseLevelYearId == yearLevelId && e.Semster == Semster);
    }
}
