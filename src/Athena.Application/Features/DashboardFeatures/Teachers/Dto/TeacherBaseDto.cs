namespace Athena.Application.Features.DashboardFeatures.Teachers.Dto
{
    public class TeacherBaseDto : IDto
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? Address { get; set; }
        public string? ImagePath { get; set; }
        public string? CourseName { get; set; } 
        public List<string>? Roles { get; set; } 
    }
}
