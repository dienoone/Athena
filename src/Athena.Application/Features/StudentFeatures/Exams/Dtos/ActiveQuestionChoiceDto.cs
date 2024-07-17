namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class ActiveQuestionChoiceDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
    }
}
