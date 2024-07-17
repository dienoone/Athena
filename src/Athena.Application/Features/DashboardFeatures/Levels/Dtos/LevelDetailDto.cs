namespace Athena.Application.Features.DashboardFeatures.Levels.Dtos
{
    public class LevelDetailDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public List<LevelClassificationsDto>? Classifications { get; set; }
    }

    
}
