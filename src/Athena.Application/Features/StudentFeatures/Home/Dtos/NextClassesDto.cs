namespace Athena.Application.Features.StudentFeatures.Home.Dtos
{
    public class NextClassesDto : IDto
    {
        public List<NextClassDto>? Today { get; set; }
        public List<NextClassDto>? Yesterday { get; set; }
        public List<NextClassDto>? Tomorrow { get; set; }

    }
}
