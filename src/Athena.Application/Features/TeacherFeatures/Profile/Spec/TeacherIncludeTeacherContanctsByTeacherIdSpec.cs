namespace Athena.Application.Features.TeacherFeatures.Profile.Spec
{
    public class TeacherIncludeTeacherContanctsByTeacherIdSpec : Specification<Teacher>, ISingleResultSpecification
    {
        public TeacherIncludeTeacherContanctsByTeacherIdSpec(Guid id) =>
            Query.Where(e => e.Id == id)
            .Include(e => e.Course)
            .Include(e => e.HeadQuarters)
            .Include(e => e.TeacherContacts.Where(e => e.DeletedOn == null));
    }
}
