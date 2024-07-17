namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class TeacherStudentListByBusinessIdSpec : Specification<Year>, ISingleResultSpecification
    {
        public TeacherStudentListByBusinessIdSpec(Guid BusinessId, string yearState) =>
            Query.Where(e => e.BusinessId == BusinessId && e.YearState == yearState && e.State);
            
    }


    /*
     .Include(e => e.TeacherCourseLevelYears.Where(e => e.DeletedOn == null)).ThenInclude(e => e.TeacherCourseLevel.Level)
            .Include(e => e.TeacherCourseLevelYears.Where(e => e.DeletedOn == null)).ThenInclude(e => e.TeacherCourseLevelYearStudents.Where(e => e.DeletedOn == null))
            .ThenInclude(e => e.Student);
     
     */

}
