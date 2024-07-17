namespace Athena.Application.Common.Interfaces
{
    public interface ISignalRConnectionService : ITransientService
    {
        Task<SignalRConnection> CreateConnection(Guid userId, string connectionId, string type, Guid? businessId);
        Task<SignalRConnectionGroup> CreateConnectionGroup(Guid connectionId, string group);

        Task<IEnumerable<SignalRConnection>> GetConnectionsForGroup(string group, CancellationToken cancellationToken);

        Task<SignalRConnection> DeleteConnection(string connectionId);
    }
}
