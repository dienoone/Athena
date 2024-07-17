namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos
{
    public class JoinRequestLevelDto : IDto
    {
        public string? LevelName { get; set; }
        public List<JoinStudentRequestDto>? Students { get; set; }
    }
}
