namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class GroupScadualByDayAndGroupIdSpec : Specification<GroupScadual>, ISingleResultSpecification
    {
        public GroupScadualByDayAndGroupIdSpec(string day, Guid groupId) =>
            Query.Where(e => e.Day == day && e.GroupId == groupId);

    }
}
