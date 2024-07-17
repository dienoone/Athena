namespace Athena.Application.Features.TeacherFeatures.Home.Dtos
{
    public class TeacherHomeRequestDto : IDto
    {
        public LastResultDto? LastResult { get; set; }
        public List<UpcommingLessonsDto>? UpcommingLessons { get; set; }
        public TeacherHomeExamDto? Exams { get; set; }
        public TeacherHomeExamReportDto? ExamReport { get; set; }
    }
}
