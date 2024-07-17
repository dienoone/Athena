namespace Athena.Application.Features.DashboardFeatures.Levels.Spec
{
    public class LevelWithLevelClassififcationsSpec : Specification<Level>
    {
        public LevelWithLevelClassififcationsSpec() =>
            Query
            .Include(e => e.LevelClassifications)
            .ThenInclude(e => e.EducationClassification)
            .OrderBy(e => e.Index);


    }
}
