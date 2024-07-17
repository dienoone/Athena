namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamResultDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public string? Type { get; set; }

        public List<ExamResultGroupDto>? Groups { get; set; }
    }
}
