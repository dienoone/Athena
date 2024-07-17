namespace Athena.Application.Features.StudentFeatures.Students.Dtos
{
    public class StudentDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Address { get; set; }
        public string ParentName { get; set; } = default!;
        public string ParentJob { get; set; } = default!;
        public string ParentPhone { get; set; } = default!;
        public string HomePhone { get; set; } = default!;
    }
}
