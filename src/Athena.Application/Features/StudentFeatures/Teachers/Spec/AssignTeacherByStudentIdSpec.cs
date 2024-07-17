namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class AssignTeacherByStudentIdSpec : Specification<Teacher>
    {
        public AssignTeacherByStudentIdSpec(Guid studentId) =>
            Query
                .Where(teacher =>
                    teacher.TeacherCourseLevels
                        .Any(tcl => tcl.TeacherCourseLevelYears
                            .Any(tcly => tcly.Year.State &&
                                tcly.TeacherCourseLevelYearStudents
                                    .Any(tclys => tclys.StudentId == studentId))))
                .Include(e => e.Course);

    }
}
