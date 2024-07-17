namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec.GroupStudentSpec
{
    public class GroupStudentByStudentIdAndBusinessIdSpec : Specification<GroupStudent>, ISingleResultSpecification
    {
        public GroupStudentByStudentIdAndBusinessIdSpec(Guid teacherCourseLevelYearStudentId, Guid businessId) =>
            Query.Where(e => e.TeacherCourseLevelYearStudent.Id == teacherCourseLevelYearStudentId && e.BusinessId == businessId);
    }
}
