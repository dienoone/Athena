namespace Athena.Application.Features.TeacherFeatures.Home.Dtos
{
    public class LastResultDto : IDto
    {
        public int AssignedStudents { get; set; }
        public int ActiveStudents { get; set; }
        public int Groups { get; set; }
    }
}
