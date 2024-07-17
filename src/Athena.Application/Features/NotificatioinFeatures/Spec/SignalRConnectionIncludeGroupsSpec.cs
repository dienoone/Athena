namespace Athena.Application.Features.NotificatioinFeatures.Spec
{
    public class SignalRConnectionIncludeGroupsSpec : Specification<SignalRConnection>
    {
        public SignalRConnectionIncludeGroupsSpec() =>
            Query.Include(e => e.Groups);
    }
}
