namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class ExamListByStudentIdAndStartDateSpec : Specification<Exam>
    {
        public ExamListByStudentIdAndStartDateSpec(Guid studentId, DateTime start) =>
            Query.Where(e => e.PublishedDate >= start && e.PublishedDate < start.AddMonths(1).AddSeconds(-1) &&
                e.ExamGroups.Any(eg => eg.DeletedOn == null &&
                    eg.ExamGroupStudents.Any(egs => egs.DeletedOn == null &&
                        egs.GroupStudent.TeacherCourseLevelYearStudent.StudentId == studentId)));


    }
}
