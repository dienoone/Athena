using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class PenddingRequestByStudentIdAndTeacherIdSpec : Specification<StudentTeacherRequest>, ISingleResultSpecification
    {
        public PenddingRequestByStudentIdAndTeacherIdSpec(Guid teacherId, Guid studentId) =>
            Query.Where(e => e.TeacherId == teacherId && e.StudentId == studentId && e.State == StudentTeacherRequestStatus.Pending);
    }
}
