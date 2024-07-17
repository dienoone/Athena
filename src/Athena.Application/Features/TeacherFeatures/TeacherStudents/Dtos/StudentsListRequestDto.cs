namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos
{
    public class StudentsListRequestDto : IDto
    {
        public List<TeacherStudentYearListDto>? Open { get; set; }
        public List<TeacherStudentYearListDto>? Preopen { get; set; }

    }
}
