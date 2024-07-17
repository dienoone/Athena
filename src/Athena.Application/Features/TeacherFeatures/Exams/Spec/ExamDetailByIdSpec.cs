namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamDetailByIdSpec : Specification<Exam>, ISingleResultSpecification
    {
        public ExamDetailByIdSpec(Guid Id)
        {
            Query.Where(e => e.Id == Id)
                .Include(e => e.ExamType)
                .Include(e => e.TeacherCourseLevelYear.TeacherCourseLevel.Level)
                .Include(e => e.TeacherCourseLevelYear.Year)
                .ThenInclude(e => e.DashboardYear);
        }
            

            
                
    }
}
