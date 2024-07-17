namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class QuestionDetailDto : IDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string? Name { get; set; } 
        public string? Type { get; set; } 
        public string? Answer { get; set; }
        public double Degree { get; set; }
        public bool IsPrime { get; set; }

        public List<QuestionChoiceDetailDto>? Choices { get; set; }
        public List<ImageDetailDto>? Images { get; set; }
    }
}
