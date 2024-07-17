namespace Athena.Application.Features.DashboardFeatures.Levels.Spec
{
    public class LevelByNameSpec : Specification<Level>, ISingleResultSpecification
    {
        public LevelByNameSpec(string name) =>
            Query.Where(e => e.Name == name);

    }
}
