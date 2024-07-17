namespace Athena.Application.Features.TeacherFeatures.Profile.Dtos
{
    public class ProfileTeacherDto : IDto
    {
        // profile card:
        public string? Image { get; set; }
        public string? CoverImage { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public string? Course { get; set; }
        public DateTime? BirthDay { get; set; }
        public List<string>? HeadQuarters { get; set; }
        public string? Nationality { get; set; }
        public string? School { get; set; }
        public string? Degree { get; set; }
        public string? TeachingMethod { get; set; }

        // contact card:
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? WebSite { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Youtube { get; set; }

        // summary card
        public string? Summary { get; set; }
    }
}
