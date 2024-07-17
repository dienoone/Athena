namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class TeacherContactDetailsDto : IDto
    {
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Youtube { get; set; }
        public string? Website { get; set; }
    }
}
