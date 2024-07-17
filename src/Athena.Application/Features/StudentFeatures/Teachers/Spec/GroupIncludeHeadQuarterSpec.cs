namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class GroupIncludeHeadQuarterSpec : Specification<GroupStudent>, ISingleResultSpecification
    {
        public GroupIncludeHeadQuarterSpec(Guid studentId, Guid teacherId) =>
            Query.Where(e => e.TeacherCourseLevelYearStudent.StudentId == studentId
                && e.Group.TeacherCourseLevelYear.TeacherCourseLevel.TeacherId == teacherId)
                    .Include(e => e.Group).ThenInclude(e => e.HeadQuarter)
                    .Include(e => e.TeacherCourseLevelYearStudent.TeacherCourseLevelYear);

    }
}
