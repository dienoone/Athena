namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class StudentTeacherRequestPhasesByStudentTeacherRequestSpec : Specification<StudentTeacherRequest>
    {
        public StudentTeacherRequestPhasesByStudentTeacherRequestSpec(Guid studentId, Guid teacherId) =>
            Query.Where(e => e.StudentId == studentId && e.TeacherId == teacherId);
    }
}
