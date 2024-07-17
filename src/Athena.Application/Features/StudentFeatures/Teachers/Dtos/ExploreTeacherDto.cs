namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class ExploreTeacherDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public Guid CourseId { get; set; }
        public string? Course { get; set; }
    }
}