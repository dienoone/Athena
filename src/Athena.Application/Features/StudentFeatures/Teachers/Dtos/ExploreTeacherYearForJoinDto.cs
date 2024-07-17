namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class ExploreTeacherYearForJoinDto : IDto
    {
        public Guid TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? TeacherImage { get; set; }
        public string? Course { get; set; }

        public ExploreTeacherYearDto? Open { get; set; }
        public ExploreTeacherYearDto? Preopen { get; set; }
    }

    public class ExploreTeacherRequestReviewDto : ExploreTeacherYearForJoinDto, IDto
    {
        public StudentChoiceDto? StudentChoice { get; set; }

    }

    public class StudentChoiceDto : IDto
    {
        public Guid YearId { get; set; }
        public Guid GroupId { get; set; }
    }
}
