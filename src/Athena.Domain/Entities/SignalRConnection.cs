namespace Athena.Domain.Entities
{
    public class SignalRConnection : BaseEntity, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public string ConnectionId { get; private set; } = default!;
        public string Type { get; private set; } = default!;
        public Guid? BusinessId { get; private set; }
        public bool IsConnected { get; private set; }
        public virtual ICollection<SignalRConnectionGroup> Groups { get; private set; } = default!;

        public SignalRConnection(Guid userId, string connectionId, string type, Guid? businessId, bool isConnected)
        {
            UserId = userId;
            ConnectionId = connectionId;
            Type = type;
            BusinessId = businessId;
            IsConnected = isConnected;
        }
    }
}
