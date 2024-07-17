namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class TeacherByTeacherIdIncludeDetailsSpec : Specification<Teacher>, ISingleResultSpecification
    {
        public TeacherByTeacherIdIncludeDetailsSpec(Guid id) =>
            Query.Where(e => e.Id == id)
                .Include(e => e.HeadQuarters.Where(h => h.DeletedOn == null))
                .ThenInclude(hp => hp.HeadQuarterPhones.Where(e => e.DeletedOn == null))
                .Include(e => e.TeacherContacts.Where(e => e.DeletedOn == null))
                .Include(e => e.Course);
    }
}
