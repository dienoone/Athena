namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec
{
    public class GroupsByTeacherCourseLevelYearIdSpec : Specification<Group>
    {
        public GroupsByTeacherCourseLevelYearIdSpec(Guid teacherCourseLevelYearId) =>
            Query.Where(e => e.TeacherCourseLevelYearId == teacherCourseLevelYearId);
    }
}
