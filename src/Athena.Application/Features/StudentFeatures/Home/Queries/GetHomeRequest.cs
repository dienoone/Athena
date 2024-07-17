using Athena.Application.Features.StudentFeatures.Home.Dtos;
using Athena.Application.Features.StudentFeatures.Home.Spec;
using Athena.Application.Features.StudentFeatures.Teachers.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Domain.Common.Const;
using System.Globalization;

namespace Athena.Application.Features.StudentFeatures.Home.Queries
{
    public record GetHomeRequest() : IRequest<HomeRequestDto>;

    public class GetHomeRequestHandler : IRequestHandler<GetHomeRequest, HomeRequestDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<GroupStudent> _groupStudentRepo;
        private readonly IReadRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;

        public GetHomeRequestHandler(
            ICurrentUser currentUser,
            IReadRepository<Exam> examRepo,
            IReadRepository<Student> studentRepo,
            IReadRepository<Teacher> teacherRepo,
            IReadRepository<GroupStudent> groupStudentRepo,
            IReadRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo)
        {
            _currentUser = currentUser;
            _examRepo = examRepo;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
            _groupStudentRepo = groupStudentRepo;
            _teacherCourseLevelYearStudentRepo = teacherCourseLevelYearStudentRepo;
        }

        public async Task<HomeRequestDto> Handle(GetHomeRequest request, CancellationToken cancellationToken)
        {
            Guid studentId = _currentUser.GetUserId();
            var teacherCourseLevelYearStudentList = await GetTeacherCourseLevelYearStudents(studentId, cancellationToken);

            TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, egyptTimeZone);

            string[] dayNames =
            {
                egyptTime.ToString("dddd", CultureInfo.InvariantCulture),
                egyptTime.AddDays(1).ToString("dddd", CultureInfo.InvariantCulture),
                egyptTime.AddDays(-1).ToString("dddd", CultureInfo.InvariantCulture),
            };

            foreach(var dayName in dayNames)
            {
                await Console.Out.WriteLineAsync(dayName);
            }

            var today = new List<NextClassDto>();
            var tomorrow = new List<NextClassDto>();
            var yesterday = new List<NextClassDto>();

            if(teacherCourseLevelYearStudentList != null)
            {
                foreach (var teacherCourseLevelYearStudent in teacherCourseLevelYearStudentList)
                {
                    AddClassIfNotNull(today, await GetNextClassDtoAsync(teacherCourseLevelYearStudent, dayNames[0], cancellationToken));
                    AddClassIfNotNull(tomorrow, await GetNextClassDtoAsync(teacherCourseLevelYearStudent, dayNames[1], cancellationToken));
                    AddClassIfNotNull(yesterday, await GetNextClassDtoAsync(teacherCourseLevelYearStudent, dayNames[2], cancellationToken));
                }
            }

            var upcommingExams = await GetUpcomingExams(teacherCourseLevelYearStudentList, cancellationToken);
            var exploreTeacherDtos = await GetExploreTeachers(cancellationToken);

            var nextClasses = new NextClassesDto
            {
                Today = today,
                Yesterday = yesterday,
                Tomorrow = tomorrow
            };

            return new HomeRequestDto
            {
                NextClasses = nextClasses,
                UpcommingExams = upcommingExams,
                ExploreTeachers = exploreTeacherDtos
            };
        }

        private async Task<List<TeacherCourseLevelYearStudent>?> GetTeacherCourseLevelYearStudents(Guid studentId, CancellationToken cancellationToken)
        {
            return await _teacherCourseLevelYearStudentRepo
                .ListAsync(new TeacherCourseLevelStudentListByStudentIdSpec(studentId), cancellationToken);
        }

        private static void AddClassIfNotNull(List<NextClassDto> classList, NextClassDto? nextClass)
        {
            if (nextClass != null)
            {
                classList.Add(nextClass);
            }
        }

        private async Task<List<UpcommingExamDto>> GetUpcomingExams(List<TeacherCourseLevelYearStudent>? teacherCourseLevelYearStudentList, CancellationToken cancellationToken)
        {
            var examStates = new[] { ExamState.Active, ExamState.Finished, ExamState.Upcoming };
            var upcommingExams = new List<UpcommingExamDto>();

            if(teacherCourseLevelYearStudentList != null)
            {
                foreach (var teacherCourseLevelYearStudent in teacherCourseLevelYearStudentList)
                {
                    var upcommingExamList = await _examRepo
                        .ListAsync(new UpcommingExamsByTeacherCourseLevelYearStudentIdSpec(teacherCourseLevelYearStudent.Id, examStates), cancellationToken);

                    upcommingExams.AddRange(upcommingExamList.Select(upcoming => new UpcommingExamDto
                    {
                        Id = upcoming.Id,
                        Exam = upcoming.Name,
                        State = upcoming.State,
                        Image = teacherCourseLevelYearStudent.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.ImagePath,
                        Teacher = teacherCourseLevelYearStudent.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.Name,
                    }));
                }
            }

            return upcommingExams;
        }

        private async Task<List<ExploreTeacherDto>> GetExploreTeachers(CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetBySpecAsync(new GetLevelByStudentIdSpec(_currentUser.GetUserId()), cancellationToken);
            var exploreTeachers = await _teacherRepo
                .ListAsync(new SmallExploreTeachersByLeveIdAndStudentIdSpec(student!.Id, student.LevelClassification.Level.Id), cancellationToken);

            return exploreTeachers.Select(exploreTeacher => new ExploreTeacherDto
            {
                Id = exploreTeacher.Id,
                Name = exploreTeacher.Name,
                Image = exploreTeacher.ImagePath,
                CourseId = exploreTeacher.CourseId,
                Course = exploreTeacher.Course.Name
            }).ToList();
        }

        private async Task<NextClassDto?> GetNextClassDtoAsync(TeacherCourseLevelYearStudent teacherCourseLevelYearStudent, string dayName, CancellationToken cancellationToken)
        {
            var groupStudent = await _groupStudentRepo
                .GetBySpecAsync(new GroupStudentByTeacherCourseLevelYearStudentIdSpec(teacherCourseLevelYearStudent.Id, dayName), cancellationToken);

            if (groupStudent == null) return null;

            return new NextClassDto
            {
                Course = teacherCourseLevelYearStudent.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.Course.Name,
                TeacherName = teacherCourseLevelYearStudent.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.Name,
                Time = groupStudent?.Group.GroupScaduals.FirstOrDefault()?.StartTime,
                Image = teacherCourseLevelYearStudent.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.ImagePath
            };
        }

    }

}

