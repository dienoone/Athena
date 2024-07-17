namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class TeacherCourseLevelYearStudentByStudentIdSpec : Specification<TeacherCourseLevelYearStudent>, ISingleResultSpecification
    {
        public TeacherCourseLevelYearStudentByStudentIdSpec(Guid studentId, Guid businessId) =>
            Query.Where(e => e.StudentId == studentId && e.BusinessId == businessId);
    }
}
