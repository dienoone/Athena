namespace Athena.Application.Features.TeacherFeatures.Home.Spec
{
    public class SignalRConnectionGroupByGroupNameSpec : Specification<SignalRConnectionGroup>
    {
        public SignalRConnectionGroupByGroupNameSpec(string groupName) =>
            Query.Where(e => e.Name == groupName);
    }
}
