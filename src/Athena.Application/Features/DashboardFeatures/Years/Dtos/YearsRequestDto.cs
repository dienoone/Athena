namespace Athena.Application.Features.DashboardFeatures.Years.Dtos
{
    public class YearsRequestDto
    {
        public List<DashboardYearDto>? Open  { get; set; }
        public List<DashboardYearDto>? Preopen  { get; set; }
        public List<DashboardYearDto>? Finished  { get; set; }

    }
}
