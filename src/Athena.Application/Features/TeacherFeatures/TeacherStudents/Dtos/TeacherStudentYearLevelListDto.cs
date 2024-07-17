namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos
{
    public class TeacherStudentYearLevelListDto : IDto
    {
        public Guid TeacherCourseLevelId { get; set; }
        public string? LevelName { get; set; }

        public List<TeacherStudentYearLevelStudentDto>? Students { get; set; }
    }
}
