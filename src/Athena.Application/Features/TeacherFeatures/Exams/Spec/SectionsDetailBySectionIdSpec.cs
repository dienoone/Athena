namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class SectionsDetailBySectionIdSpec : Specification<Section>, ISingleResultSpecification
    {
        public SectionsDetailBySectionIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.SectionImages!.Where(e => e.DeletedOn == null));
    }
}
