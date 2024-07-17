using Athena.Application.Features.StudentFeatures.Teachers.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;

namespace Athena.Application.Features.StudentFeatures.Teachers.Queries
{
    public record GetTeachersRequest : IRequest<TeachersDto>;

    public class GetTeachersRequestHandler : IRequestHandler<GetTeachersRequest, TeachersDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<GroupStudent> _groupStudentRepo;

        public GetTeachersRequestHandler(
            ICurrentUser currentUser, 
            IReadRepository<Student> studentRepo, 
            IReadRepository<Teacher> teacherRepo, 
            IReadRepository<GroupStudent> groupStudentRepo)
        {
            _currentUser = currentUser;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
            _groupStudentRepo = groupStudentRepo;
        }

        public async Task<TeachersDto> Handle(GetTeachersRequest request, CancellationToken cancellationToken)
        {
            var student = await GetStudentAsync(cancellationToken);
            var assignedTeachers = await GetAssignedTeachersAsync(student.Id, cancellationToken);
            var exploreTeachers = await GetExploreTeachersAsync(student, cancellationToken);

            List<AssignedTeacherDto> assignedTeacherDtos = new();
            List<ExploreTeacherDto> exploreTeacherDtos = new();

            if(assignedTeachers != null)
            {
                foreach (var assignTeacher in assignedTeachers)
                {
                    AssignedTeacherDto assignedTeacherDto = await MapToAssignedTeacherDto(assignTeacher, student, cancellationToken);
                    assignedTeacherDtos.Add(assignedTeacherDto);
                }
            }

            if(exploreTeachers != null)
            {
                foreach (var exploreTeacher in exploreTeachers)
                {
                    
                    ExploreTeacherDto exploreTeacherDto = MapToExploreTeacherDto(exploreTeacher);
                    exploreTeacherDtos.Add(exploreTeacherDto);
                }
            }

            return new TeachersDto()
            {
                AssignedTeachers = assignedTeacherDtos,
                ExploreTeachers = exploreTeacherDtos
            };

        }

        private async Task<Student> GetStudentAsync(CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetBySpecAsync(new GetLevelByStudentIdSpec(_currentUser.GetUserId()), cancellationToken);
            _ = student ?? throw new ConflictException($"Student Not Found!");
            return student;
        }
        private async Task<List<Teacher>> GetAssignedTeachersAsync(Guid studentId, CancellationToken cancellationToken)
        {
            return await _teacherRepo.ListAsync(new AssignTeacherByStudentIdSpec(studentId), cancellationToken);
        }
        private async Task<List<Teacher>> GetExploreTeachersAsync(Student student, CancellationToken cancellationToken)
        {
            return await _teacherRepo.ListAsync(
                new SmallExploreTeachersByLeveIdAndStudentIdSpec(student.Id, student.LevelClassification.Level.Id),
                cancellationToken);
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
        private async Task<AssignedTeacherDto> MapToAssignedTeacherDto(Teacher teacher, Student student, CancellationToken cancellationToken)
        {
            var group = await _groupStudentRepo.GetBySpecAsync(new GroupIncludeHeadQuarterSpec(student.Id, teacher.Id), cancellationToken);
            return new AssignedTeacherDto
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Image = teacher.ImagePath,
                Coures = teacher.Course.Name,
                Group = group!.Group.Name,
                HeadQuarter = group.Group.HeadQuarter.Name,
                MonghlyFee = group.TeacherCourseLevelYearStudent.TeacherCourseLevelYear.MonthFee 
            };
        }

    }
}