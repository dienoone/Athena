namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class GroupByNameAndTeacherCourseLevelYearIdSpec : Specification<Group>, ISingleResultSpecification
    {
        public GroupByNameAndTeacherCourseLevelYearIdSpec(string name, Guid teacherCourseLevelYearId) =>
            Query.Where(e => e.Name == name && e.TeacherCourseLevelYearId == teacherCourseLevelYearId);
    }
}
