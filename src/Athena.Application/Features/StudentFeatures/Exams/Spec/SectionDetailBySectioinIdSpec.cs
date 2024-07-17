namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class SectionDetailBySectioinIdSpec : Specification<Section>, ISingleResultSpecification
    {
        public SectionDetailBySectioinIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.SectionImages);
        
    }
}
