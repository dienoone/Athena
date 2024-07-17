namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class StudentTeacherCommunicationByStudentIdAndTeacherIdSpec : Specification<StudentTeacherCommunication>, ISingleResultSpecification
    {
        public StudentTeacherCommunicationByStudentIdAndTeacherIdSpec(Guid studentId, Guid teacherId) =>
            Query.Where(e => e.TeacherId == teacherId && e.StudentId == studentId);
    }
}
