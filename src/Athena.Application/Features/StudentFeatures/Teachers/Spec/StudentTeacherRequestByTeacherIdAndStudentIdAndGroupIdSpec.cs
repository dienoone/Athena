namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class StudentTeacherRequestByTeacherIdAndStudentIdAndGroupIdSpec : Specification<StudentTeacherRequest>
    {
        public StudentTeacherRequestByTeacherIdAndStudentIdAndGroupIdSpec(Guid teacherId, Guid studentId, Guid groupId) =>
            Query.Where(e => e.StudentId == studentId && e.TeacherId == teacherId && e.GroupId == groupId);
    }
}
