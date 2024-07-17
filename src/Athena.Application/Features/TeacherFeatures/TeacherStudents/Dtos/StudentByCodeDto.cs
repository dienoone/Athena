using Athena.Application.Features.TeacherFeatures.Groups.Dtos;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos
{
    public class StudentByCodeDto : IDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? LevelName { get; set; }
        public string? UserName { get; set; }
        public string? EducationClassificationName { get; set; }
        public string? Email { get; set; }

        public string? Image { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? HomePhone { get; set; }
        public string? ParentName { get; set; }
        public string? ParentJob { get; set; }
        public string? ParentPhone { get; set; }

        public List<GroupsRequiredDto>? Open { get; set; }
        public List<GroupsRequiredDto>? PreOpen { get; set; }

    }
}
