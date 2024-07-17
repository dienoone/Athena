namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class ActiveQuestionDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double Degree { get; set; }
        public bool IsPrime { get; set; }
        public string? StudentAnswer { get; set; }

        public List<ActiveQuestionChoiceDto>? Choices { get; set; }
        public List<ActiveImageDetailDto>? Images { get; set; }
    }
}
