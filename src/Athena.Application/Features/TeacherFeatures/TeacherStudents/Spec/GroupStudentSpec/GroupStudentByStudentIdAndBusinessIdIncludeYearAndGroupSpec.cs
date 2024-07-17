namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec.GroupStudentSpec
{
    public class GroupStudentByStudentIdAndBusinessIdIncludeYearAndGroupSpec : Specification<GroupStudent>, ISingleResultSpecification
    {
        public GroupStudentByStudentIdAndBusinessIdIncludeYearAndGroupSpec(Guid studentId, Guid businessId) =>
            Query.Where(e => e.TeacherCourseLevelYearStudent.StudentId == studentId && e.BusinessId == businessId)
            .Include(e => e.TeacherCourseLevelYearStudent.TeacherCourseLevelYear)
            .Include(e => e.Group);
    }
}
