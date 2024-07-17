namespace Athena.Application.Features.DashboardFeatures.Courses.Dtos
{
    public class CourseDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } 
    }
}
