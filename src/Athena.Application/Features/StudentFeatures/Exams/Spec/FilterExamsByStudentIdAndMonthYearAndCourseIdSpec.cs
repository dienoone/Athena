namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class FilterExamsByStudentIdAndMonthYearAndCourseIdSpec : Specification<Exam>
    {
        public FilterExamsByStudentIdAndMonthYearAndCourseIdSpec(Guid studentId, DateTime start, Guid? courseId)
        {
            if (courseId != null)
            {
                Query.Where(e => e.PublishedDate >= start && e.PublishedDate < start.AddMonths(1).AddSeconds(-1) &&
                    e.ExamGroups.Any(eg => eg.DeletedOn == null &&
                        eg.ExamGroupStudents.Any(egs => egs.DeletedOn == null &&
                            egs.GroupStudent.TeacherCourseLevelYearStudent.StudentId == studentId &&
                            egs.GroupStudent.Group.TeacherCourseLevelYear.TeacherCourseLevel.Teacher.CourseId== courseId)));
            }
            else
            {
                Query.Where(e => e.PublishedDate >= start && e.PublishedDate < start.AddMonths(1).AddSeconds(-1) &&
                    e.ExamGroups.Any(eg => eg.DeletedOn == null &&
                        eg.ExamGroupStudents.Any(egs => egs.DeletedOn == null &&
                            egs.GroupStudent.TeacherCourseLevelYearStudent.StudentId == studentId)));
            }
        }
    }
}
