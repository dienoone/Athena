namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class ExamListDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public Guid CourseId { get; set; }
        public string? Course { get; set; }
        public string? TeacherName { get; set; }
        public string? TeacherImage { get; set; }
        public DateTime? Date { get; set; }
    }
}
