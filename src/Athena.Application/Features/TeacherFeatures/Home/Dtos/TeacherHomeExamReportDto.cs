namespace Athena.Application.Features.TeacherFeatures.Home.Dtos
{
    public class TeacherHomeExamReportDto : IDto
    {
        public MonthExamReportDto? January { get; set; }
        public MonthExamReportDto? February { get; set; }
        public MonthExamReportDto? March { get; set; }
        public MonthExamReportDto? April { get; set; }
        public MonthExamReportDto? May { get; set; }
        public MonthExamReportDto? June { get; set; }
        public MonthExamReportDto? July { get; set; }
        public MonthExamReportDto? August { get; set; }
        public MonthExamReportDto? September { get; set; }
        public MonthExamReportDto? October { get; set; }
        public MonthExamReportDto? November { get; set; }
        public MonthExamReportDto? December { get; set; }

    }
}
