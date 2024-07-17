namespace Athena.Application.Features.StudentFeatures.Profile.Dtos
{
    public class StudentProfileDto : IDto
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set;}
        public string? Code { get; set; }
        public string? School { get; set; }
        public string? Level { get; set; }
        public string? Classification { get; set; }
        public string? Image { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Address { get; set; }
        public string? ParentName { get; set; }
        public string? ParentJob { get; set; }
        public string? ParentPhone { get; set; }
        public Guid LevelClassificationId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? HomePhone { get; set; }
    }
}
