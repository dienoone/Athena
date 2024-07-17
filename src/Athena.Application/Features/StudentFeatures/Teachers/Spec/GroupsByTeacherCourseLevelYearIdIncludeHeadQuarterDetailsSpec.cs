namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class GroupsByTeacherCourseLevelYearIdIncludeHeadQuarterDetailsSpec : Specification<Group>
    {
        public GroupsByTeacherCourseLevelYearIdIncludeHeadQuarterDetailsSpec(Guid teacherCourseLevelYearId) =>
            Query.Where(e => e.TeacherCourseLevelYearId == teacherCourseLevelYearId && e.DeletedOn == null)
            .Include(e => e.GroupScaduals.Where(e => e.DeletedOn == null))
            .Include(e => e.HeadQuarter)
            .ThenInclude(e => e.HeadQuarterPhones.Where(e => e.DeletedOn == null));
    }
}
