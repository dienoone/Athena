namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos
{
    public class ExamResultsDto : IDto
    {
        public Guid ExamId { get; set; }
        public string? ExamName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool Attendance { get; set; }
        public string? State { get; set; }
        public double ExamDegree { get; set; }
        public double StudentDegree { get; set; }
        public double Percentage { get; set; }
        public int Points { get; set; }
    }
}
