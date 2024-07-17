namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class SmallExploreTeachersByLeveIdAndStudentIdAndCourseIdSpec : Specification<Teacher>
    {
        public SmallExploreTeachersByLeveIdAndStudentIdAndCourseIdSpec(Guid studentId, Guid levelId, Guid courseId) =>
            Query
                .Where(teacher => teacher.CourseId == courseId && 
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
