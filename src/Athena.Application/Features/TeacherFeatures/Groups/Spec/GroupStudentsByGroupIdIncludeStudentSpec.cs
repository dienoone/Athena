namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class GroupStudentsByGroupIdIncludeStudentSpec : Specification<GroupStudent>
    {
        public GroupStudentsByGroupIdIncludeStudentSpec(Guid groupId) =>
            Query.Where(e => e.GroupId == groupId)
                .Include(e => e.TeacherCourseLevelYearStudent.Student);
    }
}
