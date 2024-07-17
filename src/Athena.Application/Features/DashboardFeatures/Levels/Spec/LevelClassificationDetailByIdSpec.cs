namespace Athena.Application.Features.DashboardFeatures.Levels.Spec
{
    public class LevelClassificationDetailByIdSpec : Specification<LevelClassification>, ISingleResultSpecification
    {
        public LevelClassificationDetailByIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
            .Include(e => e.Level)
            .Include(e => e.EducationClassification);
    }
}
