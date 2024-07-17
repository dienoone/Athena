namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class TeacherIncludeContactsAndCourseByTeacherIdSpec : Specification<Teacher>, ISingleResultSpecification
    {
        public TeacherIncludeContactsAndCourseByTeacherIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.Course)
                .Include(e => e.TeacherContacts.Where(e => e.DeletedOn == null));
    }
}
