namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamCorrectionRoomDto : IDto
    {
        public Guid ExamId { get; set; }
        public string? Name { get; set; }
        public bool StartCorrect { get; set; }
        public bool IsFinished { get; set; }
        public List<ExamCorrectionRoomGoupDto>? Groups { get; set; }
    }
}
