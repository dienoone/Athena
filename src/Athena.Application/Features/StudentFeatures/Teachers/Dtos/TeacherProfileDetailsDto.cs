namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class TeacherProfileDetailsDto : IDto
    {
        public string? CoverImage { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public Guid CourseId { get; set; }
        public string? Course { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Address { get; set; }
        public string? Nationality { get; set; }
        public string? School { get; set; }
        public string? Education { get; set; }
        public string? TeachingTypes { get; set; }
    }
}
