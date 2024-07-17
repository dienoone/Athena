namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class StudentTeacherRequestByTeacherIdAndStudentIdSpec : Specification<StudentTeacherRequest>
    {
        public StudentTeacherRequestByTeacherIdAndStudentIdSpec(Guid teacherId, Guid studentId) =>
            Query.Where(e => e.TeacherId == teacherId && e.StudentId == studentId);
                
    }
}
