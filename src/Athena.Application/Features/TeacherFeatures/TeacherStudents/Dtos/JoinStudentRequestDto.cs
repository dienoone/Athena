namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos
{
    public class JoinStudentRequestDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Image { get; set; }
        public string? GroupName { get; set; }
        public string? YearState { get; set; }
    }
}
