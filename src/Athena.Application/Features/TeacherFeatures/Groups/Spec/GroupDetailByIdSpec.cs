namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class GroupDetailByIdSpec : Specification<Group>, ISingleResultSpecification
    {
        public GroupDetailByIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
            .Include(e => e.GroupScaduals.Where(e => e.DeletedOn == null))
            .Include(e => e.GroupStudents.Where(e => e.DeletedOn == null))
            .ThenInclude(e => e.TeacherCourseLevelYearStudent)
            .Include(e => e.HeadQuarter)
            .Include(e => e.TeacherCourseLevelYear.Year.DashboardYear)
            .Include(e => e.TeacherCourseLevelYear.TeacherCourseLevel.Level);

    }
}
