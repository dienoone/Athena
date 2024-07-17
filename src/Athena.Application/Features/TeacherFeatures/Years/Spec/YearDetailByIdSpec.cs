namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class YearDetailByIdSpec : Specification<Year>, ISingleResultSpecification
    {
        public YearDetailByIdSpec(Guid Id) =>
            Query
                .Where(e => e.Id == Id)
                .Include(e => e.DashboardYear)
                .Include(e => e.TeacherCourseLevelYears.Where(e => e.DeletedOn == null))
                    .ThenInclude(e => e.TeacherCourseLevel.Level)
                .Include(e => e.TeacherCourseLevelYears.Where(e => e.DeletedOn == null))
                    .ThenInclude(e => e.TeacherCourseLevelYearSemsters.Where(e => e.DeletedOn == null))
                .OrderBy(e => e.TeacherCourseLevelYears.Min(y => y.TeacherCourseLevel.Level.Index));
    }
}
