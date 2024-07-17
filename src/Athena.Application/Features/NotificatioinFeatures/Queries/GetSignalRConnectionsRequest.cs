using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.NotificatioinFeatures.Spec;

namespace Athena.Application.Features.NotificatioinFeatures.Queries
{
    public record GetSignalRConnectionsRequest() : IRequest<List<SignalRConnectionDto>>;

    public class GetSignalRConnectionsRequestHandler : IRequestHandler<GetSignalRConnectionsRequest, List<SignalRConnectionDto>>
    {
        private readonly IReadRepository<SignalRConnection> _signalRConnectionRepo;

        public GetSignalRConnectionsRequestHandler(IReadRepository<SignalRConnection> signalRConnectionRepo)
        {
            _signalRConnectionRepo = signalRConnectionRepo;
        }

        public async Task<List<SignalRConnectionDto>> Handle(GetSignalRConnectionsRequest request, CancellationToken cancellationToken)
        {
            List<SignalRConnectionDto> dtos = new();

            var connections = await _signalRConnectionRepo.ListAsync(new SignalRConnectionIncludeGroupsSpec(), cancellationToken);

            foreach (var connection in connections)
            {
                SignalRConnectionDto dto = new() 
                { 
                    Id = connection.Id,
                    ConnectionId = connection.ConnectionId,
                    BusinessId = connection.BusinessId,
                    Groups = connection.Groups.Select(e => e.Name)
                };
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
