namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamListDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public string? Level  { get; set; }
        public string? ExamType { get; set; }
        public bool IsPrime { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
