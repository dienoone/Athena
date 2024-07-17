namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamCorrectionRoomStudentDto : IDto
    {
        public Guid Id { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? GroupName { get; set; }
        public int FinalDegree { get; set; }
        public int StudentDegree { get; set; }
        public int Percentage { get; set; }
        public string? State { get; set; }
        public bool IsFinish { get; set; }
    }
}
