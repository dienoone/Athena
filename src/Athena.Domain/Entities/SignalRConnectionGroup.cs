namespace Athena.Domain.Entities
{
    public class SignalRConnectionGroup : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; } = default!;
        public Guid ConnectionId { get; set; }
        public virtual SignalRConnection Connection { get; private set; } = default!;

        public SignalRConnectionGroup(string name, Guid connectionId)
        {
            Name = name;
            ConnectionId = connectionId;
        }
    }
}
