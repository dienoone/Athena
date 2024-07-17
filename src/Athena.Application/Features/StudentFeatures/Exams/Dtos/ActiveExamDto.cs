namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class ActiveExamDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Course { get; set; }
        public string? TeacherName { get; set; }
        public string? TeacherImage { get; set; }
        public string? ExamType { get; set; }
        public double FinalDegree { get; set; }
        public double AllowedTime { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int NumberOfSections { get; set; }
        public DateTime PublishedDate { get; set; }
        public TimeSpan PublishedTime { get; set; }
        public string? State { get; set; }
        public bool IsPrime { get; set; }
    }
}
