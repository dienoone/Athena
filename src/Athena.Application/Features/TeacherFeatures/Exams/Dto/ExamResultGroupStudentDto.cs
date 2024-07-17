namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamResultGroupStudentDto : IDto
    {
        public Guid Id { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? GroupName { get; set; }
        public int FinalDegree { get; set; }
        public double StudentDegree { get; set; }
        public int Percentage { get; set; }
        public string? State { get; set; }
    }
}
