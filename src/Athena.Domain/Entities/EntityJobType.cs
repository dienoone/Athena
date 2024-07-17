namespace Athena.Domain.Entities
{
    public class EntityJobType : BaseEntity, IAggregateRoot
    {
        public Guid EntityId { get; private set; }
        public string Type { get; private set; }

        public EntityJobType(Guid id, Guid entityId, string type)
        {
            Id = id;
            EntityId = entityId;
            Type = type;
        }
    }
}
