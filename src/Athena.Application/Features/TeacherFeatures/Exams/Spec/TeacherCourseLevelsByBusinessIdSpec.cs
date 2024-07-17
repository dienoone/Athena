using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class TeacherCourseLevelsByBusinessIdSpec : Specification<TeacherCourseLevelYear>
    {
        public TeacherCourseLevelsByBusinessIdSpec(Guid businessId) =>
            Query.Where(e => e.BusinessId == businessId && e.Year.YearState == YearStatus.Open && e.Year.State)
                .Include(e => e.TeacherCourseLevel.Level)
                .OrderBy(e => e.TeacherCourseLevel.Level.Index);
    }
}
