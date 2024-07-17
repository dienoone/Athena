namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class TeacherCourseLevelStudentsByBusinessIdSpec : Specification<TeacherCourseLevelYearStudent>
    {
        public TeacherCourseLevelStudentsByBusinessIdSpec(Guid businessId) =>
            Query.Where(e => e.BusinessId == businessId)
                .Include(e => e.Student).ThenInclude(e => e.LevelClassification.Level)
                .Include(e => e.Student).ThenInclude(e => e.LevelClassification.EducationClassification)
                .Include(e => e.TeacherCourseLevelYear.TeacherCourseLevel.Level);
    }
}
