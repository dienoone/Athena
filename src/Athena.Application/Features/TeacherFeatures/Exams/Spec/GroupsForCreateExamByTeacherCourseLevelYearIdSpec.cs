namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class GroupsForCreateExamByTeacherCourseLevelYearIdSpec : Specification<Group>
    {
        public GroupsForCreateExamByTeacherCourseLevelYearIdSpec(Guid teacherCourseLevelYearId) =>
            Query.Where(e => e.TeacherCourseLevelYearId == teacherCourseLevelYearId);
    }
}
