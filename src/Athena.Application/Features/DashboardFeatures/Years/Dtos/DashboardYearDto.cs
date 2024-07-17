namespace Athena.Application.Features.DashboardFeatures.Years.Dtos
{
    public class DashboardYearDto : IDto
    {
        public Guid Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string? State { get; set; }
        public bool IsFinished { get; set; }
    }
}
