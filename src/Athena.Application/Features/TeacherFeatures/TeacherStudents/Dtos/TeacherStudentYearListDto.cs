namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos
{
    public class TeacherStudentYearListDto : IDto
    {
        public string? LevelName { get; set; }
        public List<TeacherStudentYearLevelStudentDto>? Students { get; set; }
    }
}
