namespace Athena.Application.Features.DashboardFeatures.Notifications.Commands
{
    public record DeleteNotificationTypeTemplateByIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteNotificationTypeTemplateByIdRequestHandler : IRequestHandler<DeleteNotificationTypeTemplateByIdRequest, Guid>
    {
        private readonly IRepository<NotificationTypeTemplate> _templateRepo;
        private readonly IStringLocalizer<DeleteNotificationTypeTemplateByIdRequestHandler> _t;

        public DeleteNotificationTypeTemplateByIdRequestHandler(IRepository<NotificationTypeTemplate> templateRepo, 
            IStringLocalizer<DeleteNotificationTypeTemplateByIdRequestHandler> t)
        {
            _templateRepo = templateRepo;
            _t = t;
        }

        public async Task<Guid> Handle(DeleteNotificationTypeTemplateByIdRequest request, CancellationToken cancellationToken)
        {
            var template = await _templateRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = template ?? throw new NotFoundException(_t["NotificationTypeTemplate {0} Not Found!", request.Id]);

            await _templateRepo.DeleteAsync(template, cancellationToken);
            return request.Id;
        }
    }
}
