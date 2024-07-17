namespace Athena.Application.Features.DashboardFeatures.Levels.Dtos
{
    public class LevelClassificationsDto : IDto
    {
        public Guid LevelClassificationId { get; set; }
        public string? Name { get; set; }
    }
}
