using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Home.Spec
{
    public class TeacherCourseLevelYearStudentsByBusinessIdSpec : Specification<TeacherCourseLevelYearStudent>
    {
        public TeacherCourseLevelYearStudentsByBusinessIdSpec(Guid businessId) =>
            Query.Where(e => e.TeacherCourseLevelYear.Year.State && e.TeacherCourseLevelYear.Year.YearState == YearStatus.Open && e.BusinessId == businessId);
    }
}
