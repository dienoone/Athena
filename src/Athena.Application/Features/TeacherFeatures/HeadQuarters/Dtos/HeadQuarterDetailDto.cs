namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Dtos
{
    public class HeadQuarterDetailDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public List<HeadQuarterPhoneDto>? Phones { get; set; }

        public List<HeadQuarterLevelsDto>? Open { get; set; }
        public List<HeadQuarterLevelsDto>? Preopen { get; set; }
    }
}
