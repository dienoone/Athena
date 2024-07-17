namespace Athena.Application.Features.StudentFeatures.Students.Dtos
{
    public class StudentBaseDto : IDto
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? Address { get; set; }
        public string? ImagePath { get; set; }
        public List<string>? CourseNames { get; set; }
        public string? Code { get; set; }

    }
}
