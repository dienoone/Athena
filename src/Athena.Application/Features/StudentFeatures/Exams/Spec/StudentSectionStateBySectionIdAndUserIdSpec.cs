namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class StudentSectionStateBySectionIdAndUserIdSpec : Specification<StudentSectionState>, ISingleResultSpecification
    {
        public StudentSectionStateBySectionIdAndUserIdSpec(Guid sectionId, Guid userId) =>
            Query.Where(e => e.StudentId == userId && e.SectionId == sectionId);
    }
}
