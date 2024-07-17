namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class QuestionChoiceDetailDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public bool IsRightChoice { get; set; }
    }

    public class QuestionChoiceDetailResultDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public bool IsRightChoice { get; set; }
    }
}
