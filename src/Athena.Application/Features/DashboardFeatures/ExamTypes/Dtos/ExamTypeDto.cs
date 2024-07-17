namespace Athena.Application.Features.DashboardFeatures.ExamTypes.Dtos
{
    public class ExamTypeDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
