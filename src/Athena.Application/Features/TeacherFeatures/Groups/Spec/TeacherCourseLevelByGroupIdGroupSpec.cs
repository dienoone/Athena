namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class TeacherCourseLevelByGroupIdGroupSpec : Specification<Group>, ISingleResultSpecification
    {
        public TeacherCourseLevelByGroupIdGroupSpec(Guid groupId) =>
            Query.Where(e => e.Id == groupId)
                .Include(e => e.TeacherCourseLevelYear);

    }
}
