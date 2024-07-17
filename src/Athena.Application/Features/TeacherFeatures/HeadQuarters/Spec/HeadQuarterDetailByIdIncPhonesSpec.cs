namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec
{
    public class HeadQuarterDetailByIdIncPhonesSpec : Specification<HeadQuarter>, ISingleResultSpecification
    {
        public HeadQuarterDetailByIdIncPhonesSpec(Guid id) =>
            Query.Where(e => e.Id == id)
                .Include(e => e.HeadQuarterPhones.Where(e => e.DeletedOn == null));
    }
}
