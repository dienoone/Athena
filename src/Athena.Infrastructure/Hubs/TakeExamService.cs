using Athena.Application.Features.TeacherFeatures.Exams.Commands.Create;
using Microsoft.AspNetCore.SignalR;

namespace Athena.Infrastructure.Hubs
{
    public class TakeExamService : ITakeExamService
    {
        private readonly IHubContext<TakeExamHub> _takeExamHubContext;

        public TakeExamService(IHubContext<TakeExamHub> takeExamHubContext)
        {
            _takeExamHubContext = takeExamHubContext;
        }

        public Task DeleteFromGroupAsync(string group, string connectionId, CancellationToken cancellationToken) =>
            _takeExamHubContext.Groups.RemoveFromGroupAsync(connectionId, group);

        public Task NotifyEndExamAsync(string group, CancellationToken cancellationToken) =>
            _takeExamHubContext.Clients.Group(group).SendAsync("NotifyEndExam", cancellationToken);
        
    }
}
