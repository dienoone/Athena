namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamStudentAnswerSectionDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string? Name { get; set; }
        public string? Paragraph { get; set; }
        public double Degree { get; set; }
        public bool IsPrime { get; set; }

        public List<ExamStudentAnswerQuestionDto>? Questions { get; set; }
        public List<ImageDetailDto>? Images { get; set; }

    }
}
