namespace Athena.Application.Features.StudentFeatures.TimeTable.Dtos
{
    public class TimeTableRequestDto
    {
        public List<DayScheduleDto>? Saturday { get; set; }
        public List<DayScheduleDto>? Sunday { get; set; }
        public List<DayScheduleDto>? Monday { get; set; }
        public List<DayScheduleDto>? TuesDay { get; set; }
        public List<DayScheduleDto>? Wednesday { get; set; }
        public List<DayScheduleDto>? Thursday { get; set; }
        public List<DayScheduleDto>? Friday { get; set; }

        public List<TeacherGroupScheduleDto>? TeachersSchedule { get; set; }
    }
}
