namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class StudentTeacherRequestByIdIncludeYearSpec : Specification<StudentTeacherRequest>, ISingleResultSpecification
    {
        public StudentTeacherRequestByIdIncludeYearSpec(Guid id) =>
            Query.Where(e => e.Id == id)
                .Include(e => e.Group.TeacherCourseLevelYear.Year);
    }
}
