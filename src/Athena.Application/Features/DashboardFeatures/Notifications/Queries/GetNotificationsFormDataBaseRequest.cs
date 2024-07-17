namespace Athena.Application.Features.DashboardFeatures.Notifications.Queries
{
    public record GetNotificationsFormDataBaseRequest() : IRequest<List<Notification>>;

    public class NotificationSepc : Specification<Notification>
    {
        public NotificationSepc() =>
            Query
                .Include(e => e.NotificationType)
                .Include(e => e.NotificationMessages.Where(e => e.DeletedOn == null))
                .Include(e => e.NotificationRecipients.Where(e => e.DeletedOn == null));
    }

    public class GetNotificationsFormDataBaseRequestHandler : IRequestHandler<GetNotificationsFormDataBaseRequest, List<Notification>>
    {
        private readonly IReadRepository<Notification> _repository;

        public GetNotificationsFormDataBaseRequestHandler(IReadRepository<Notification> repository)
        {
            _repository = repository;
        }

        
        public async Task<List<Notification>> Handle(GetNotificationsFormDataBaseRequest request, CancellationToken cancellationToken)
        {
            return await _repository.ListAsync(cancellationToken);
        }
    }
}
