namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class StudentAnswerDto : IDto
    {
        public Guid Id { get; set; }
        public string? ExamName { get; set; }
        public string? StudentName { get; set; }
        public string? Gender { get; set; }
        public double StudentDegree { get; set; }

        public List<ExamStudentAnswerSectionDto>? Sections { get; set; }
    }

    
}
