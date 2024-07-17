namespace Athena.Domain.Entities
{
    public class QuestionChoice : AuditableEntity, IAggregateRoot
    {
        public int Index { get; private set; }
        public string Name { get; private set; } = default!;
        public string? Image { get; private set; }
        public bool IsRightChoice { get; private set; }
        public Guid QuestionId { get; private set; }
        public virtual Question Question { get; private set; } = default!;

        public QuestionChoice(int index, string name, string? image, bool isRightChoice, Guid questionId, Guid businessId)
        {
            Index = index;
            Name = name;
            Image = image;
            IsRightChoice = isRightChoice;
            QuestionId = questionId;
            BusinessId = businessId;
        }

        public QuestionChoice Update(int? index, string? name, string? image, bool? isRightChoice)
        {
            if (index is not null && Index.Equals(index) is not true) Index = (int)index;
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (image is not null && Image?.Equals(image) is not true) Image = image;
            if (isRightChoice is not null && IsRightChoice.Equals(isRightChoice) is not true) IsRightChoice = (bool)isRightChoice;
            return this;
        }

        public QuestionChoice ClearImagePath()
        {
            Image = string.Empty;
            return this;
        }
    }
}
