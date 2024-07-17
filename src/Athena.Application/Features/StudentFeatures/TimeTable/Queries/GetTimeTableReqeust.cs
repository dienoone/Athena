using Athena.Application.Features.StudentFeatures.TimeTable.Dtos;
using Athena.Application.Features.StudentFeatures.TimeTable.Spec;

namespace Athena.Application.Features.StudentFeatures.TimeTable.Queries
{
    public record GetTimeTableReqeust() : IRequest<TimeTableRequestDto>;

    public class GetTimeTableReqeustHandler : IRequestHandler<GetTimeTableReqeust, TimeTableRequestDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;
        private readonly IReadRepository<Group> _groupRepo;

        public GetTimeTableReqeustHandler(
            ICurrentUser currentUser, 
            IReadRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo,
            IReadRepository<Group> groupRepo)
        {
            _currentUser = currentUser;
            _teacherCourseLevelYearStudentRepo = teacherCourseLevelYearStudentRepo;
            _groupRepo = groupRepo;
        }

        public async Task<TimeTableRequestDto> Handle(GetTimeTableReqeust request, CancellationToken cancellationToken)
        {

            var teacherCourseLevelYearStudents = await _teacherCourseLevelYearStudentRepo.ListAsync(new TeacherCourseLevelYearStudentByStudentIdSpec(_currentUser.GetUserId()), cancellationToken);
            List<TeacherGroupScheduleDto> teacherGroupSchedules = new();
            
            foreach (var teacherCourseLevelYearStudent in teacherCourseLevelYearStudents)
            {
                var teacher = teacherCourseLevelYearStudent.TeacherCourseLevelYear.TeacherCourseLevel.Teacher;
                var group = await _groupRepo.GetBySpecAsync(new GroupsByTeacherIdAndTeacherCourseLevelYearStudentIdSpec(teacher.Id, teacherCourseLevelYearStudent.Id), cancellationToken);
                TeacherGroupScheduleDto dto = new() 
                { 
                    Id = teacher.Id,
                    Image = teacher.ImagePath,
                    Teacher = teacher.Name,
                    Course = teacher.Course.Name,
                    GroupId = group!.Id,
                    GroupName = group!.Name,
                    Scaduals = group.GroupScaduals.Adapt<List<GroupScheduleDto>>()
                };
                teacherGroupSchedules.Add(dto);
            }

            return new()
            {
                Saturday = MapDaySchedule("Saturday", teacherGroupSchedules),
                Sunday = MapDaySchedule("Sunday", teacherGroupSchedules),
                Monday = MapDaySchedule("Monday", teacherGroupSchedules),
                TuesDay = MapDaySchedule("Tuesday", teacherGroupSchedules),
                Wednesday = MapDaySchedule("Wednesday", teacherGroupSchedules),
                Thursday = MapDaySchedule("Thursday", teacherGroupSchedules),
                Friday = MapDaySchedule("Friday", teacherGroupSchedules),
                TeachersSchedule = teacherGroupSchedules
            };
        }

        private static List<DayScheduleDto> MapDaySchedule(string dayName, List<TeacherGroupScheduleDto> groupSchedules)
        {
            List<DayScheduleDto> dtos = new();

            foreach (var groupSchedule in groupSchedules)
            {
                var schedules = groupSchedule.Scaduals!.Where(e => e.Day ==  dayName);
                if (schedules.Any())
                {
                    foreach(var schedule in schedules)
                    {
                        DayScheduleDto dto = new()
                        {
                            GroupId = groupSchedule.GroupId,
                            GroupName = groupSchedule.GroupName,
                            Course = groupSchedule.Course,
                            Teacher = groupSchedule.Teacher,
                            Image = groupSchedule.Image,
                            StartTime = schedule.StartTime,
                            Day = schedule.Day,
                        };
                        dtos.Add(dto);
                    }
                }
            }
            
            return dtos;
        }
    }

}
