namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class TeacherHeadquartersDetailsDto : IDto 
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; } 
        public string? Region { get; set; }
        public string? Street { get; set; } 
        public string? Building { get; set; }

        public List<TeacherHeadquarterPhonesDetailsDto>? Phones { get; set; }

    }
}
