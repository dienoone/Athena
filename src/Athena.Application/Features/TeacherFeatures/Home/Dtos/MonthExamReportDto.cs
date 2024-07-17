namespace Athena.Application.Features.TeacherFeatures.Home.Dtos
{
    public class MonthExamReportDto : IDto
    {
        public int Distinctive { get; set; }
        public int Successed { get; set; }
        public int Failed { get; set; }
        public int Absent { get; set; }

    }
}
