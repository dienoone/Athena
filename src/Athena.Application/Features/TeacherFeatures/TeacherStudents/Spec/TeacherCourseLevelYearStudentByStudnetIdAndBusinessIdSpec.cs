namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class TeacherCourseLevelYearStudentByStudnetIdAndBusinessIdSpec : Specification<TeacherCourseLevelYearStudent>, ISingleResultSpecification
    {
        public TeacherCourseLevelYearStudentByStudnetIdAndBusinessIdSpec(Guid studentId, Guid businessId) =>
            Query.Where(e => e.StudentId == studentId && e.BusinessId == businessId 
            && e.TeacherCourseLevelYear.Year.DeletedOn == null && e.TeacherCourseLevelYear.Year.State);
    }
}
