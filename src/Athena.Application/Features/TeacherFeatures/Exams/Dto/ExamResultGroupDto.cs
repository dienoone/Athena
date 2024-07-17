namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamResultGroupDto : IDto
    {
        public string? Name { get; set; }
        public List<ExamResultGroupStudentDto>? Students { get; set; }
    }
}
