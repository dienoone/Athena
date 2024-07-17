namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class SmallExploreTeacherSpec : Specification<TeacherCourseLevelYear>
    {
        public SmallExploreTeacherSpec(Guid levelId, List<Guid>? teacherIds)
        {
            if (teacherIds == null || teacherIds.Count == 0)
            {
                Query.Where(e => e.TeacherCourseLevel.LevelId == levelId && e.Year.State)
                     .Include(e => e.TeacherCourseLevel.Teacher).ThenInclude(e => e.Course)
                     .Take(5);
            }
            else
            {
                Query.Where(e => e.TeacherCourseLevel.LevelId == levelId && !(teacherIds.Contains(e.TeacherCourseLevel.TeacherId)))
                   .Include(e => e.TeacherCourseLevel.Teacher).ThenInclude(e => e.Course)
                   .Take(5);
            }
        }
    }
}
