using Athena.Application.Common.Exceptions;
using Athena.Application.Features.StudentFeatures.Exams.Commands;
using Athena.Application.Features.StudentFeatures.Exams.Queries;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Web.Http;

namespace Athena.Infrastructure.Hubs
{
    [Authorize]
    public class TakeExamHub : Hub, ITransientService
    {
        private readonly ILogger<TakeExamHub> _logger;
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IUserService _userService;
        private readonly ISignalRConnectionService _signalRConnectionService;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<TakeExamHub> _t;

        public TakeExamHub(
            ILogger<TakeExamHub> logger,
            IReadRepository<Exam> readRepo,
            IUserService userService,
            ISignalRConnectionService signalRConnectionService,
            IMediator mediator,
            IStringLocalizer<TakeExamHub> t)
        {
            _logger = logger;
            _examRepo = readRepo;
            _userService = userService;
            _signalRConnectionService = signalRConnectionService;
            _mediator = mediator;
            _t = t;
        }

        #region HandleConnections:

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            await ValidateRoles(userId);

            var examId = GetExamId();
            await ValidateExam(examId);

            var connection = await _signalRConnectionService.CreateConnection(GetUserId(), Context.ConnectionId, EHubTypes.TakeExam.ToString(), null);
            await AddToGroup(connection.Id, $"{EHubTypes.TakeExam}-{examId}");


            await base.OnConnectedAsync();

            _logger.LogInformation("A client connected to TakeExamHub: {connectionId}", Context.ConnectionId);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var examId = GetExamId();

            await RemoveFromGroup($"{EHubTypes.TakeExam}-{examId}");

            await _signalRConnectionService.DeleteConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);

            _logger.LogInformation("A client disconnected from Takeexam: {connectionId}", Context.ConnectionId);
        }

        #endregion

        public async Task GetExamDetails()
        {
            var exam = await _mediator.Send(new GetActiveExamByIdRequest(GetExamId(), GetUserId()));
            await Clients.Caller.SendAsync("ExamDetails", exam);
        }

        public async Task GetActiveSections()
        {
            var sections = await _mediator.Send(new GetActiveSectionsByExamIdAndUserIdRequest(GetExamId(), GetUserId()));
            await Clients.Caller.SendAsync("ActiveSections", sections);
        }

        public async Task GetSectionDetailsById(Guid sectionId)
        {
            var sectionDto = await _mediator.Send(new GetActiveSectionBySectionIdReqeust(sectionId, GetUserId()));
            await Clients.Caller.SendAsync("SectionDetails", sectionDto);
        }

        public async Task AnswerSectionById(AnswerSectionRequest request)
        {
            request.UserId = GetUserId();
            var section = await _mediator.Send(request);
            await Clients.Caller.SendAsync("AnswerSection", section);
        }

        public async Task EndExam()
        {
            await RemoveFromGroup($"{EHubTypes.TakeExam}-{GetExamId()}");
            await Clients.Caller.SendAsync("NotifyEndExam");
        }


        #region Helpers:

        private async Task ValidateRoles(Guid userId)
        {
            var roles = await _userService.GetRolesAsync(userId.ToString(), new());

            if (!roles.Contains("Student"))
                throw new UnauthorizedException("Authentication Failed.");
        }
        private async Task ValidateExam(Guid examId)
        {
            var exam = await _examRepo.GetByIdAsync(examId);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", examId]);
        }

        private Guid GetUserId()
        {
            var userId = Context.User?.GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException(_t["Authentication Failed."]);

            return Guid.Parse(userId);
        }
        private Guid GetExamId()
        {
            var examId = Context.GetHttpContext()?.Request.Query["examId"].ToString();
            if (string.IsNullOrEmpty(examId))
                throw new InternalServerException(_t["Can't start connection without examId"]);

            return Guid.Parse(examId);
        }


        private async Task AddToGroup(Guid connectionId, string groupName)
        {
            await _signalRConnectionService.CreateConnectionGroup(connectionId, groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        private async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        #endregion

    }
}
