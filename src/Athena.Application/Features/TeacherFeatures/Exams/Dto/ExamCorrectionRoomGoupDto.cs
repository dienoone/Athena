namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamCorrectionRoomGoupDto : IDto
    {
        public string? Name { get; set; }
        public List<ExamCorrectionRoomStudentDto>? Students { get; set; }
    }
}
