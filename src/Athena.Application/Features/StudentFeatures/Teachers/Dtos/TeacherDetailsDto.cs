namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class TeacherDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public TeacherProfileDetailsDto? Details { get; set; }
        public TeacherContactDetailsDto? ContactDetails { get; set; }
        public TeacherTimeTableDto? TimeTable { get; set; }

    }
}
