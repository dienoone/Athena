namespace Athena.Application.Features.TeacherFeatures.Years.Dtos
{
    public class YearListDto : IDto
    {
        public Guid Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public bool State { get; set; }
        public string? YearState { get; set; }

    }
}
