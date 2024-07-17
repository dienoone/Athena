using Athena.Application.Features.DashboardFeatures.ExamTypes.Dtos;

namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    // ToDo: Add TeacherCourseLevels
    public class CreateExamRequiredDto : IDto
    {
        public List<ExamTypeDto>? ExamTypes { get; set; }
        public List<TeacherCourseLevelYearRequiredToCreateExamDto>? Levels { get; set; }
    }
}
