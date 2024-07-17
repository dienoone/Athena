namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class SectionDetailDto : IDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string? Name { get; set; }
        public string? Paragraph { get; set; }
        public double Degree { get; set; }
        public bool IsPrime { get; set; }
        public int? Time { get; set; }

        public List<QuestionDetailDto>? Questions { get; set; }
        public List<ImageDetailDto>? Images { get; set; }
    }
}
