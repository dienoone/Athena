using Athena.Application.Features.DashboardFeatures.Courses.Dtos;
using Athena.Application.Features.StudentFeatures.Exams.Dtos;
using Athena.Application.Features.StudentFeatures.Exams.Spec;

namespace Athena.Application.Features.StudentFeatures.Exams.Queries
{
    public record GetExamsRequest() : IRequest<ExamsRequestDto>;
    public class GetExamsRequestHandler : IRequestHandler<GetExamsRequest, ExamsRequestDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Exam> _examRepo;
        
        public GetExamsRequestHandler(
            ICurrentUser currentUser,
            IReadRepository<Exam> examRepo)
        {
            _currentUser = currentUser;
            _examRepo = examRepo;
        }

        public async Task<ExamsRequestDto> Handle(GetExamsRequest request, CancellationToken cancellationToken)
        {
            var studentId = _currentUser.GetUserId();

            var exams = await _examRepo.ListAsync(new ExamListByStudentIdSpec(studentId), cancellationToken);
            var examDtos = MapExamDtos(exams);
            
            return new() 
            {   
                Courses = MapCourses(examDtos),
                Exams = examDtos,
                FilterRange = MapFilterRange(examDtos)
            };
        }

        #region Helpers:

        private static List<ExamListDto>? MapExamDtos(IEnumerable<Exam>? exams)
        {
            if (exams == null)
                return null;

            return exams.Select(exam =>
                new ExamListDto
                {
                    Id = exam.Id,
                    Name = exam.Name,
                    State = exam.State,
                    CourseId = exam.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.CourseId,
                    Course = exam.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.Course.Name,
                    TeacherName = exam.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.Name,
                    TeacherImage = exam.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.ImagePath,
                    Date = exam.PublishedDate
                })
                .ToList();
        }

        private static List<CourseDto>? MapCourses(IEnumerable<ExamListDto>? examDtos)
        {
            if (examDtos == null)
                return null;

            return examDtos.Select(e =>
                new CourseDto
                {
                    Id = e.CourseId,
                    Name = e.Course
                })
                .Distinct().ToList();
        }

        private static FilterRangeDto? MapFilterRange(IEnumerable<ExamListDto>? examDtos)
        {
            if (examDtos == null)
                return null;

            return new()
            {
                StartDate = examDtos.Min(e => e.Date),
                EndDate = examDtos.Max(e => e.Date)
            };
        }

        #endregion
    }
}
