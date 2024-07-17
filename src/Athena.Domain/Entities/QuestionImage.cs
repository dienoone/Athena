namespace Athena.Domain.Entities
{
    public class QuestionImage : AuditableEntity, IAggregateRoot
    {
        public string Image { get; private set; } = default!;
        public int Index { get; private set; } = default!;
        public Guid QuestionId { get; private set; }
        public virtual Question Question { get; private set; } = default!;

        public QuestionImage(string image, int index, Guid questionId, Guid businessId)
        {
            Image = image;
            Index = index;
            QuestionId = questionId;
            BusinessId = businessId;
        }

        public QuestionImage Update(string? image, int? index)
        {
            if (image is not null && Image?.Equals(image) is not true) Image = image;
            if (index is not null && Index.Equals(index) is not true) Index = (int)index;
            return this;
        }

        public QuestionImage ClearImagePath()
        {
            Image = string.Empty;
            return this;
        }
    }
}
