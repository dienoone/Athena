namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class ExploreTeachersByLevelClassificationIdAndStudentIdSpec : Specification<Teacher>
    {
        public ExploreTeachersByLevelClassificationIdAndStudentIdSpec(Guid studentId, Guid levelId) =>
            Query
                .Where(teacher => 
                    !teacher.TeacherCourseLevels
                        .Any(tcl => tcl.TeacherCourseLevelYears
                            .Any(tcly => tcly.Year.State &&
                                tcly.TeacherCourseLevel.LevelId == levelId &&
                                tcly.TeacherCourseLevelYearStudents
                                    .Any(tclys => tclys.StudentId == studentId))))
                .Include(e => e.Course);
    }
}
