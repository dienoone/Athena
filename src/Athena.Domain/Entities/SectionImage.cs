namespace Athena.Domain.Entities
{
    public class SectionImage : AuditableEntity, IAggregateRoot
    {
        public string Image { get; private set; } = default!;
        public int Index { get; private set; }
        public Guid SectionId { get; private set; }
        public virtual Section Section { get; private set; } = default!;

        public SectionImage(string image, int index, Guid sectionId, Guid businessId)
        {
            Image = image;
            Index = index;
            SectionId = sectionId;
            BusinessId= businessId;
        }

        public SectionImage Update(string? image, int? index) 
        {
            if (image is not null && Image?.Equals(image) is not true) Image = image;
            if (index is not null && Index.Equals(index) is not true) Index = (int)index;
            return this;
        }

        public SectionImage ClearImagePath()
        {
            Image = string.Empty;
            return this;
        }
    }
}
