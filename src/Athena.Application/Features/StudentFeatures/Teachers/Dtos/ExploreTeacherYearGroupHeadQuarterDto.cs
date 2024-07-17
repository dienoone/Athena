namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class ExploreTeacherYearGroupHeadQuarterDto : IDto
    {
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }

        public List<ExploreTeacherYearGroupHeadQuarterPhoneDto>? Phones { get; set; }
    }

    public class ExploreTeacherYearGroupHeadQuarterPhoneDto : IDto
    {
        public string? Phone { get; set; }
    }
}
