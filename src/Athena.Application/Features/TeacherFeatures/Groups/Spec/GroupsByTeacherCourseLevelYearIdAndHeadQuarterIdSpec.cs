namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class GroupsByTeacherCourseLevelYearIdAndHeadQuarterIdSpec : Specification<Group>
    {
        public GroupsByTeacherCourseLevelYearIdAndHeadQuarterIdSpec(Guid TeacherCourseLevelYearId, Guid headId) =>
            Query.Where(e => e.TeacherCourseLevelYearId == TeacherCourseLevelYearId && e.HeadQuarterId == headId);
    }
}
