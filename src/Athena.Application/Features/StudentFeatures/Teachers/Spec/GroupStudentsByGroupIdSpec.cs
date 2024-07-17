namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class GroupStudentsByGroupIdSpec : Specification<GroupStudent>
    {
        public GroupStudentsByGroupIdSpec(Guid groupId) =>
            Query.Where(g => g.GroupId == groupId)
                .Include(e => e.TeacherCourseLevelYearStudent);
    }
}
