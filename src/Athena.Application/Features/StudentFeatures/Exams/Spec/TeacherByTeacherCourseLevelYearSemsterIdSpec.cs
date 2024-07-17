namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class TeacherByTeacherCourseLevelYearSemsterIdSpec : Specification<Teacher>, ISingleResultSpecification
    {
        public TeacherByTeacherCourseLevelYearSemsterIdSpec(Guid TeacherCourseLevelYearId) =>
            Query.Include(e => e.Course)
                .Where(e => e.TeacherCourseLevels
                    .Any(e => e.TeacherCourseLevelYears
                        .Any(e => e.Id == TeacherCourseLevelYearId)));
    }
}
