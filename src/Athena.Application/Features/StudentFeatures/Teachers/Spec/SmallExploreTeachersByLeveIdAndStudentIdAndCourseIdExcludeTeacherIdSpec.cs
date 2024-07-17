namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class SmallExploreTeachersByLeveIdAndStudentIdAndCourseIdExcludeTeacherIdSpec : Specification<Teacher>
    {
        public SmallExploreTeachersByLeveIdAndStudentIdAndCourseIdExcludeTeacherIdSpec(Guid studentId, Guid levelId, Guid courseId, Guid teacherId) =>
            Query
                .Where(teacher => teacher.CourseId == courseId && teacher.Id != teacherId &&
                    !teacher.TeacherCourseLevels
                        .Any(tcl => tcl.LevelId == levelId &&
                            tcl.TeacherCourseLevelYears
                                .Any(tcly => tcly.Year.State &&
                                    tcly.TeacherCourseLevelYearStudents
                                        .Any(tclys => tclys.StudentId == studentId))))
                .Include(e => e.Course)
                .Take(5);

    }
}
