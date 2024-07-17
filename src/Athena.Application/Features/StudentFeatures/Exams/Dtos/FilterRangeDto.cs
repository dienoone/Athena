namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class FilterRangeDto : IDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
