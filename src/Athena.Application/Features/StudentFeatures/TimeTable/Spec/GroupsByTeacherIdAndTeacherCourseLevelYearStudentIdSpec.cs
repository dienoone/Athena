namespace Athena.Application.Features.StudentFeatures.TimeTable.Spec
{
    public class GroupsByTeacherIdAndTeacherCourseLevelYearStudentIdSpec : Specification<Group>, ISingleResultSpecification
    {
        public GroupsByTeacherIdAndTeacherCourseLevelYearStudentIdSpec(Guid teacherId, Guid teacherCourseLevelYearStudentId) =>
            Query.Where(e => e.GroupStudents.Any(gs => gs.TeacherCourseLevelYearStudentId == teacherCourseLevelYearStudentId)
                    && e.TeacherCourseLevelYear.TeacherCourseLevel.TeacherId == teacherId)
                .Include(e => e.GroupScaduals.Where(e => e.DeletedOn == null));
    }
}
