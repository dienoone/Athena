namespace Athena.Application.Features.DashboardFeatures.Levels.Spec
{
    public class LevelClassificationByLevelAndClassificationSpec : Specification<LevelClassification>, ISingleResultSpecification
    {
        public LevelClassificationByLevelAndClassificationSpec(Guid levelId, Guid classificationId) =>
            Query.Where(e => e.LevelId == levelId && e.EducationClassificationId == classificationId);

    }
}
