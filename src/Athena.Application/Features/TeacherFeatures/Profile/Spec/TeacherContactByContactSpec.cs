namespace Athena.Application.Features.TeacherFeatures.Profile.Spec
{
    public class TeacherContactByContactSpec : Specification<TeacherContact>, ISingleResultSpecification
    {
        public TeacherContactByContactSpec(string contact, Guid teacherId) =>
            Query.Where(e => e.Contact.Equals(contact) && e.TeacherId == teacherId);
    }
}
