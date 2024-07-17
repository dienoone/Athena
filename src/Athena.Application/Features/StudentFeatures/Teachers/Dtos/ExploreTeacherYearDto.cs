namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class ExploreTeacherYearDto : IDto
    {
        public Guid Id { get; set; }
        public string? YearState { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int IntroFee { get; set; }
        public int MonthFee { get; set; }

        public List<ExploreTeacherYearGroupDto>? Groups { get; set; }
    }
}
