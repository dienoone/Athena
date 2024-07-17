namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class YearsByBusinessIdAndStateSpec : Specification<Year>
    {
        public YearsByBusinessIdAndStateSpec(Guid buinessId) =>
            Query.Where(e => e.BusinessId == buinessId && e.State);
    }
}
