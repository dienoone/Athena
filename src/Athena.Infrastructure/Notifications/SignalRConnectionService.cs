using Athena.Application.Common.Exceptions;
using Athena.Domain.Common.Const;
using Athena.Infrastructure.Notifications.Spec;
using Microsoft.Extensions.Localization;

namespace Athena.Infrastructure.Notifications
{
    public class SignalRConnectionService : ISignalRConnectionService
    {
        private readonly IRepository<SignalRConnection> _connectionRepo;
        private readonly IRepository<SignalRConnectionGroup> _connectionGroupRepo;
        private readonly IStringLocalizer<SignalRConnectionService> _t;

        public SignalRConnectionService(
            IRepository<SignalRConnection> connectionRepo, 
            IRepository<SignalRConnectionGroup> connectionGroupRepo, 
            IStringLocalizer<SignalRConnectionService> t)
        {
            _connectionRepo = connectionRepo;
            _connectionGroupRepo = connectionGroupRepo;
            _t = t;
        }

        public async Task<SignalRConnection> CreateConnection(Guid userId, string connectionId, string type, Guid? businessId)
        {
            if(type != EHubTypes.Notification.ToString())
            {
                var isUserExist = await _connectionRepo.GetBySpecAsync(new GetSignalRConnectionByUserIdAndTypeSpec(userId, type));
                if (isUserExist != null)
                    throw new ConflictException(_t["User is already connected"]);
            }

            SignalRConnection signalRConnection = new(userId, connectionId, type, businessId, true);
            await _connectionRepo.AddAsync(signalRConnection);

            return signalRConnection;
        }

        public async Task<SignalRConnectionGroup> CreateConnectionGroup(Guid connectionId, string group)
        {
            SignalRConnectionGroup connectionGroup = new(group, connectionId);
            await _connectionGroupRepo.AddAsync(connectionGroup);

            return connectionGroup;
        }

        public async Task<SignalRConnection> DeleteConnection(string connectionId)
        {
            var connection = await _connectionRepo.GetBySpecAsync(new GetSignalRConnectionByConnectionIdSpec(connectionId));
            _ = connection ?? throw new NotFoundException(_t["Connection not found!"]);

            await _connectionRepo.DeleteAsync(connection);

            return connection;
        }

        public async Task<IEnumerable<SignalRConnection>> GetConnectionsForGroup(string group, CancellationToken cancellationToken)
        {
            return await _connectionRepo.ListAsync(new GetSignalRConnectionsByGroupNameSpec(group), cancellationToken);
        }
    }
}
