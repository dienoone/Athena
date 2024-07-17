namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class LevelsByYearIdSpec : Specification<Year>, ISingleResultSpecification
    {
        public LevelsByYearIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.TeacherCourseLevelYears.Where(e => e.DeletedOn == null));

    }
}
