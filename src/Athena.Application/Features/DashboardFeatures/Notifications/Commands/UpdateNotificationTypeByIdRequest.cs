using Athena.Application.Features.DashboardFeatures.Notifications.Spec;

namespace Athena.Application.Features.DashboardFeatures.Notifications.Commands
{
    public record UpdateNotificationTypeByIdRequest(Guid Id, string Type, string? Description) : IRequest<Guid>;

    public class UpdateNotificationTypeByIdRequestValidator : CustomValidator<UpdateNotificationTypeByIdRequest>
    {
        public UpdateNotificationTypeByIdRequestValidator(IReadRepository<NotificationType> repo, 
            IStringLocalizer<UpdateNotificationTypeByIdRequestValidator> T)
        {
            RuleFor(e => e.Type)
                .NotEmpty()
                .NotNull()
                .WithMessage(T["Type can't be null"])
                .MustAsync(async (notificationType, type, ct) =>
                    await repo.GetBySpecAsync(new NotificationTypeByTypeSpec(type), ct)
                        is not NotificationType existingType || existingType.Id == notificationType.Id)
                .WithMessage((_, type) => T["NotificatioinType {0} already Exists", type]);
        }
    }

    public class UpdateNotificationTypeByIdRequestHandler : IRequestHandler<UpdateNotificationTypeByIdRequest, Guid>
    {
        private readonly IRepository<NotificationType> _notificationTypeRepo;
        private readonly IStringLocalizer<UpdateNotificationTypeByIdRequestHandler> _t;

        public UpdateNotificationTypeByIdRequestHandler(IRepository<NotificationType> notificationTypeRepo,
            IStringLocalizer<UpdateNotificationTypeByIdRequestHandler> t)
        {
            _notificationTypeRepo = notificationTypeRepo;
            _t = t;
        }

        public async Task<Guid> Handle(UpdateNotificationTypeByIdRequest request, CancellationToken cancellationToken)
        {
            var notificationType  = await _notificationTypeRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = notificationType ?? throw new NotFoundException(_t["NotificationType {0} Not Found!", request.Id]);

            notificationType.Update(request.Type, request.Description, false);
            await _notificationTypeRepo.UpdateAsync(notificationType, cancellationToken);

            return request.Id;
        }
    }
}
