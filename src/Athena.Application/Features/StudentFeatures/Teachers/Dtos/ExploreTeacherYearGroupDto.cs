namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class ExploreTeacherYearGroupDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Remainder { get; set; }


        public ExploreTeacherYearGroupHeadQuarterDto? HeadQuarterDto { get; set; }
        public List<ExploreTeacherYearGroupScadualDto>? Scaduals { get; set; }  
    }

    public class ExploreTeacherYearGroupScadualDto : IDto
    {
        public string? Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
