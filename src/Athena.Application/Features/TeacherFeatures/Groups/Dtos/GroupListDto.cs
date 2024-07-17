namespace Athena.Application.Features.TeacherFeatures.Groups.Dtos
{
    public class GroupListDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Level { get; set; }
        public string? HeadQuarter { get; set; }
        public int StudentsCount { get; set; }

    }
}
