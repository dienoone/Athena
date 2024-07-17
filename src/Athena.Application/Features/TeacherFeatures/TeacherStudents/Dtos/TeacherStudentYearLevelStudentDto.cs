namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos
{
    public class TeacherStudentYearLevelStudentDto : IDto
    {
        public Guid Id { get; set; }
        public string? ImagePath { get; set; }
        public string? FullName { get; set; }
        public string? LevelName { get; set; }
        public string? EducationClassificationName { get; set; }
        public string? GroupName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Code { get; set; }
    }
}
