namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class GetAvailableLevelsForYearSpec : Specification<TeacherCourseLevel>
    {
        public GetAvailableLevelsForYearSpec(List<Guid> levelIds, Guid businessId) =>
            Query.Where(e => !levelIds.Contains(e.Id) && e.BusinessId == businessId)
                .Include(e => e.Level);
    }
}
