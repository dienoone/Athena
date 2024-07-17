using Athena.Application.Features.DashboardFeatures.Courses.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;

namespace Athena.Application.Features.StudentFeatures.Teachers.Queries
{
    public record GetExploreTeachersRequest() : IRequest<ExploreTeachersDto>;
    public class GetExploreTeachersRequestHandler : IRequestHandler<GetExploreTeachersRequest, ExploreTeachersDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<Course> _courseRepo;

        public GetExploreTeachersRequestHandler(
            ICurrentUser currentUser, 
            IReadRepository<Student> studentRepo,
            IReadRepository<Teacher> teacherRepo,
            IReadRepository<Course> courseRepo)
        {
            _currentUser = currentUser;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
            _courseRepo = courseRepo;
        }


        public async Task<ExploreTeachersDto> Handle(GetExploreTeachersRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetBySpecAsync(new GetLevelByStudentIdSpec(_currentUser.GetUserId()), cancellationToken);
            var courses = await _courseRepo.ListAsync(cancellationToken);

            var exploreTeachers =
              await _teacherRepo.ListAsync(
                      new ExploreTeachersByLevelClassificationIdAndStudentIdSpec(student!.Id, student.LevelClassification.LevelId),
                               cancellationToken);

            List<ExploreTeacherDto> exploreTeacherDtos = new();

            if(exploreTeacherDtos != null)
            {
                foreach (var exploreTeacher in exploreTeachers)
                {
                    ExploreTeacherDto exploreTeacherDto = MapToExploreTeacherDto(exploreTeacher);
                    exploreTeacherDtos.Add(exploreTeacherDto);
                }
            }

            return new()
            {
                Courses = courses.Adapt<List<CourseDto>>(),
                Teachers = exploreTeacherDtos
            };

        }

        private static ExploreTeacherDto MapToExploreTeacherDto(Teacher teacher)
        {
            return new ExploreTeacherDto
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Image = teacher.ImagePath,
                CourseId = teacher.CourseId,
                Course = teacher.Course.Name
            };
        }
    }
}
