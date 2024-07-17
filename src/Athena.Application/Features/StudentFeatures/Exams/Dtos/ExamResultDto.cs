namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class ExamResultDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Course { get; set; }
        public string? Teacher { get; set; }
        public string? TeacherImage { get; set; }

        public ExamResultDegreeDto? ExamReult { get; set; }
        public List<ExamResultSectionDto>? Sections { get; set; }

    }

    public class ExamResultDegreeDto : IDto
    {
        public double ExamDegree { get; set; }
        public double StudentDegree { get; set; }
        public double Percentage { get; set; }
        public string? Status { get; set; }

    }

    public class ExamResultSectionDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Paragraph { get; set; }
        public double Degree { get; set; }
        public bool IsPrime { get; set; }

        public List<ExamResultQuestionDto>? Questions { get; set; }
        public List<ActiveImageDetailDto>? Images { get; set; }
    }

    public class ExamResultQuestionDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double Degree { get; set; }
        public bool IsPrime { get; set; }
        public string? StudentAnswer { get; set; }
        public double StudentDegree { get; set; }

        public List<ExamResultChoiceDto>? Choices { get; set; }
        public List<ActiveImageDetailDto>? Images { get; set; }
    }

    public class ExamResultChoiceDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public bool IsRightChoice { get; set; }
    }
}
