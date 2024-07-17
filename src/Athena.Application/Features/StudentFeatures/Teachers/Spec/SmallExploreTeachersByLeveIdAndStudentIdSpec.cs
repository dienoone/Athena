namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class SmallExploreTeachersByLeveIdAndStudentIdSpec : Specification<Teacher>, ISingleResultSpecification
    {
        public SmallExploreTeachersByLeveIdAndStudentIdSpec(Guid studentId, Guid levelId) =>
            Query
                .Where(teacher => 
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
