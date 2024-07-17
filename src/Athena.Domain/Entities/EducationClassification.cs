namespace Athena.Domain.Entities
{
    public class EducationClassification : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; } = default!;

        public virtual ICollection<LevelClassification> LevelClassifications { get; private set; } = default!;

        public EducationClassification(string name) => Name = name;

        public EducationClassification Update(string? name)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            return this;
        }
        
    }
}
