namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class ExploreTeacherDetailDto : IDto
    {
        public Guid Id { get; set; }
        public TeacherProfileDetailsDto? Details  { get; set; }
        public TeacherContactDetailsDto? ContactDetails { get; set; }
        public string? Summary { get; set; }
        public List<TeacherHeadquartersDetailsDto>? Headquarters { get; set; }
        public TeacherTuitionYearDetailsDto? OpenYear { get; set; }
        public TeacherTuitionYearDetailsDto? PreOpenYear { get; set; }
        public List<ExploreTeacherDto>? Teachers { get; set; }
        public bool CanSendRequest { get; set; }
        public Guid? RequestId { get; set; }
    }
    
}
