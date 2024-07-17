namespace Athena.Application.Features.TeacherFeatures.Exams.Spec;

public class ExamByNameSpec : Specification<Exam>, ISingleResultSpecification
{
    public ExamByNameSpec(string name, Guid teacherCourseLevelYearId, Guid businessId) =>
        Query.Where(e => e.Name == name && 
                         e.TeacherCourseLevelYearId == teacherCourseLevelYearId && 
                         e.BusinessId == businessId);
}