namespace Athena.Domain.Entities
{
    public class Course : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; } = default!;

        public virtual ICollection<Teacher> Teachers { get; private set; } = default!;

        public Course(string name) => Name = name;
            

        public Course Update(string? name)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            return this;
        }
    }
}
