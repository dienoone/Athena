namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    // ADD CreatedON
    public class ExamDetailDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string? LevelName { get; set; }
        public DateTime PublishedDate { get; set; }
        public TimeSpan PublishedTime { get; set; }
        public int AllowedTime { get; set; }
        public double FinalDegree { get; set; }
        public int NumberOfSections { get; set; }
        public string? Description { get; set; }
        public string? State { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrime { get; set; }
        public bool IsReady { get; set; }
        public Guid TeacherCourseLevelYearId { get; set; }
        public Guid ExamTypeId { get; set; }
        public string? ExamType { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<SectionDetailDto>? Sections { get; set; }
        public List<ExamGroupDto>? Groups { get; set; }

    }
}
