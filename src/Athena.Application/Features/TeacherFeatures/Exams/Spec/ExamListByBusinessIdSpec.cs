using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamListByBusinessIdSpec : Specification<Exam>
    {
        public ExamListByBusinessIdSpec(Guid businessId) => 
            Query.Where(e => e.BusinessId == businessId && e.TeacherCourseLevelYear.Year.State && e.TeacherCourseLevelYear.DeletedOn == null
                && e.TeacherCourseLevelYear.Year.DeletedOn == null
                && e.TeacherCourseLevelYear.Year.YearState == YearStatus.Open)
                .Include(e => e.TeacherCourseLevelYear.TeacherCourseLevel.Level)
                .Include(e => e.ExamType);
    }
}
