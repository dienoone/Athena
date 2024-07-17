namespace Athena.Application.Features.StudentFeatures.Students.Dtos
{
    public class StudentsListDto : IDto
    {
        public Guid Id { get; set; }
        public string? ImagePath { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? LevelName { get; set; }
        public string? EducationClassificationName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Code { get; set; }
    }
}
