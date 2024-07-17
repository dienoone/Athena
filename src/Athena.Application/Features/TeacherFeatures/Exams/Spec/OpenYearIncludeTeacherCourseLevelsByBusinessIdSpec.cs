using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class OpenYearIncludeTeacherCourseLevelsByBusinessIdSpec : Specification<Year>, ISingleResultSpecification
    {
        public OpenYearIncludeTeacherCourseLevelsByBusinessIdSpec(Guid businessId) =>
            Query.Where(e => e.BusinessId == businessId && e.State && e.YearState == YearStatus.Open)
                .Include(e => e.TeacherCourseLevelYears.Where(e => e.DeletedOn == null));
    }
}
