namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class GroupListSpec : Specification<Group>
    {
        public GroupListSpec(Guid BusinessId, string yearState) =>
            Query.Where(e => e.BusinessId == BusinessId 
                && e.TeacherCourseLevelYear.Year.State &&
                e.TeacherCourseLevelYear.Year.YearState == yearState)
            .Include(e => e.HeadQuarter)
            .Include(e => e.GroupStudents.Where(e => e.DeletedOn == null))
            .Include(e => e.TeacherCourseLevelYear.TeacherCourseLevel.Level);
    }
}
