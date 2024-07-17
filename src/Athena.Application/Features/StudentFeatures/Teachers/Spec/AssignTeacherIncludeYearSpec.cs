namespace Athena.Application.Features.StudentFeatures.Teachers.Spec;

public class AssignTeacherIncludeYearSpec : Specification<TeacherCourseLevelYearStudent>
{
    public AssignTeacherIncludeYearSpec(Guid studentId) =>
        Query.Where(e => e.StudentId == studentId)
            .Include(e => e.TeacherCourseLevelYear)
            .Include(e => e.TeacherCourseLevelYear.TeacherCourseLevel.Teacher)
            .ThenInclude(e => e.Course);
}