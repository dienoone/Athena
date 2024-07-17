namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class SemstersByYearIdSpec : Specification<TeacherCourseLevelYearSemster>
    {
        public SemstersByYearIdSpec(Guid Id) =>
            Query.Where(e => e.TeacherCourseLevelYear.YearId == Id);
                
    }
}
