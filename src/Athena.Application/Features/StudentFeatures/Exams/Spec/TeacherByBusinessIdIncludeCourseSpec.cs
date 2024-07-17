namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class TeacherByBusinessIdIncludeCourseSpec : Specification<Teacher>, ISingleResultSpecification
    {
        public TeacherByBusinessIdIncludeCourseSpec(Guid businessId) =>
            Query.Where(e => e.BusinessId == businessId)
                .Include(e => e.Course);
    }
}
