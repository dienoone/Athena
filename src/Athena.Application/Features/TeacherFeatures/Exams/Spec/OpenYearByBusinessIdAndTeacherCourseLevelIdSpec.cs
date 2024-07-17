using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class OpenYearByBusinessIdAndTeacherCourseLevelIdSpec : Specification<TeacherCourseLevelYear>, ISingleResultSpecification
    {
        public OpenYearByBusinessIdAndTeacherCourseLevelIdSpec(Guid busniessId, Guid levelId) =>
            Query.Where(e => e.BusinessId == busniessId && e.Year.State && e.Year.YearState == YearStatus.Open && e.Id == levelId);
    }
}