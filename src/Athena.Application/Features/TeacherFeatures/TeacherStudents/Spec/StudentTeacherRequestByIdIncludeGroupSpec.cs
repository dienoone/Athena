namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class StudentTeacherRequestByIdIncludeGroupSpec : Specification<StudentTeacherRequest>, ISingleResultSpecification
    {
        public StudentTeacherRequestByIdIncludeGroupSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.Group);
    }
}
