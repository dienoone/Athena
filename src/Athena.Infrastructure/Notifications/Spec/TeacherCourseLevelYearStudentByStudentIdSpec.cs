using Ardalis.Specification;

namespace Athena.Infrastructure.Notifications.Spec
{
    public class TeacherCourseLevelYearStudentByStudentIdSpec : Specification<TeacherCourseLevelYearStudent>
    {
        public TeacherCourseLevelYearStudentByStudentIdSpec(Guid studentId) =>
            Query.Where(e => e.StudentId == studentId && e.DeletedOn == null);
        
    }
}
