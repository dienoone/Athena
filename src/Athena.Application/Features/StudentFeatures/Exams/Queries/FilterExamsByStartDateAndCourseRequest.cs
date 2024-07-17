using Athena.Application.Features.StudentFeatures.Exams.Dtos;
using Athena.Application.Features.StudentFeatures.Exams.Spec;
using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Exams.Queries
{
    public record FilterExamsByStartDateAndCourseRequest(DateTime DateTime, Guid? CourseId) : IRequest<List<ExamListDto>>;

    public class FilterExamsByStartDateAndCourseRequestHandler : IRequestHandler<FilterExamsByStartDateAndCourseRequest, List<ExamListDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IUserService _userSevice;


        public FilterExamsByStartDateAndCourseRequestHandler(ICurrentUser currentUser, IReadRepository<Exam> examRepo,
            IReadRepository<Teacher> teacherRepo, IUserService userSevice)
        {
            _currentUser = currentUser;
            _examRepo = examRepo;
            _teacherRepo = teacherRepo;
            _userSevice = userSevice;
        }
        public async Task<List<ExamListDto>> Handle(FilterExamsByStartDateAndCourseRequest request, CancellationToken cancellationToken)
        {
            var studentId = _currentUser.GetUserId();
            var startOfMonth = new DateTime(request.DateTime.Year, request.DateTime.Month, 1);
            List<ExamListDto> examDtos = new();


            var exams = await _examRepo.ListAsync(new FilterExamsByStudentIdAndMonthYearAndCourseIdSpec(studentId, startOfMonth, request.CourseId), cancellationToken);

            foreach (var exam in exams)
            {
                var teacher = await _teacherRepo.GetBySpecAsync(new TeacherByTeacherCourseLevelYearSemsterIdSpec(exam.TeacherCourseLevelYearId), cancellationToken);
                var user = await _userSevice.GetAsync(teacher!.Id.ToString(), cancellationToken);

                // Create Dtos:
                ExamListDto dto = new()
                {
                    Id = exam.Id,
                    Name = exam.Name,
                    State = exam.State,
                    Course = teacher.Course.Name,
                    TeacherName = user.FirstName + " " + user.LastName,
                    TeacherImage = teacher.ImagePath,
                    Date = exam.PublishedDate
                };
                examDtos.Add(dto);
            }

            return examDtos;
        }
    }
}
