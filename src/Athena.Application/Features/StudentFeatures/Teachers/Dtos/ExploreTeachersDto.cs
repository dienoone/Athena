using Athena.Application.Features.DashboardFeatures.Courses.Dtos;

namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class ExploreTeachersDto : IDto
    {
        public List<CourseDto>? Courses { get; set; }
        public List<ExploreTeacherDto>? Teachers { get; set; }
    }
}
