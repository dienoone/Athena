namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class AvailableGroupsByFilterAndTeacherCourserLevelYearIdSpec : Specification<Group>
    {
        public AvailableGroupsByFilterAndTeacherCourserLevelYearIdSpec(Guid teacherCourseLevelYearId, List<Guid> groupIds) =>
            Query.Where(g => !groupIds.Contains(g.Id) && g.TeacherCourseLevelYearId == teacherCourseLevelYearId);


    }
}
