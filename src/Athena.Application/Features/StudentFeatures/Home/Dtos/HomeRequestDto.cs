using Athena.Application.Features.StudentFeatures.Teachers.Dtos;

namespace Athena.Application.Features.StudentFeatures.Home.Dtos
{
    public class HomeRequestDto : IDto
    {
        public NextClassesDto? NextClasses { get; set; }
        public List<UpcommingExamDto>? UpcommingExams { get; set; }
        public List<ExploreTeacherDto>? ExploreTeachers { get; set; }
    }
}
