namespace Athena.Application.Features.TeacherFeatures.Years.Dtos
{
    public class YearListRequestDto : IDto
    {
        public List<YearListDto>? Open { get; set; }
        public List<YearListDto>? Preopen { get; set; }
        public List<YearListDto>? Finished { get; set; }
    }
}
