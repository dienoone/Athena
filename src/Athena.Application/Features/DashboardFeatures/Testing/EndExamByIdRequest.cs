using Athena.Application.Features.TeacherFeatures.Exams.Commands.Create;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.DashboardFeatures.Testing
{
    public record EndExamByIdRequest(Guid Id) : IRequest<Guid>;

    public class EndExamByIdRequestHandler : IRequestHandler<EndExamByIdRequest, Guid>
    {
        private readonly ISignalRConnectionService _connectionService;
        private readonly ITakeExamService _takeExamService;

        public EndExamByIdRequestHandler(ISignalRConnectionService connectionService, ITakeExamService takeExamService)
        {
            _connectionService = connectionService;
            _takeExamService = takeExamService;
        }

        public async Task<Guid> Handle(EndExamByIdRequest request, CancellationToken cancellationToken)
        {
            await _takeExamService.NotifyEndExamAsync($"{EHubTypes.TakeExam}-{request.Id}", cancellationToken);
            var signalRConnections = await _connectionService.GetConnectionsForGroup($"{EHubTypes.TakeExam}-{request.Id}", cancellationToken);
            foreach (var connection in signalRConnections)
            {
                await _connectionService.DeleteConnection(connection.ConnectionId);
                await _takeExamService.DeleteFromGroupAsync($"{EHubTypes.TakeExam}-{request.Id}", connection.ConnectionId, cancellationToken);
            }

            return request.Id;
        }
    }
}
