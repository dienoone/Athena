using Athena.Application.Features.DashboardFeatures.Courses.Dtos;

namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class ExamsRequestDto : IDto
    {
        public List<CourseDto>? Courses { get; set; }
        public List<ExamListDto>? Exams { get; set; }
        public FilterRangeDto? FilterRange { get; set; }

    }
}
