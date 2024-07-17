namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class GroupStudentsByGroupIdSpec : Specification<GroupStudent>
    {
        public GroupStudentsByGroupIdSpec(Guid groupId) =>
            Query.Where(e => e.GroupId == groupId)
                .Include(e => e.TeacherCourseLevelYearStudent);
    }
}
