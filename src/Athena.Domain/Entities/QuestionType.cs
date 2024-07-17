namespace Athena.Domain.Entities
{
    public class QuestionType : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; } = default!;
    }
}
