namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class TeacherTuitionYearLevelDetailsDto : IDto
    {
        public string? LevelName { get; set; }
        public double DownPayment { get; set; }
        public double MonthlyFees { get; set; }
    }
}
