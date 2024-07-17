using Athena.Application.Features.DashboardFeatures.Notifications.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.DashboardFeatures.Notifications.Commands
{
    public record CreateNotificationTypeTemplateRequest(Guid Id, string Content, string Language) : IRequest<Guid>;

    public class CreateNotificationTypeTemplateRequestValidator : CustomValidator<CreateNotificationTypeTemplateRequest>
    {
        public CreateNotificationTypeTemplateRequestValidator(IReadRepository<NotificationTypeTemplate> repo,
            IStringLocalizer<CreateNotificationTypeTemplateRequestValidator> T)
        {
            RuleFor(e => e.Language)
                .NotEmpty()
                .NotNull()
                .Must((_, language) => language == Languages.Arabic || language == Languages.English)
                .WithMessage((_, language) => T["Invalid Language: {0}", language])
                .MustAsync(async (request, language, ct) =>
                    await repo.GetBySpecAsync(
                        new NotificationTypeTemplateByNotificationTypeIdAndLanguageSpec(request.Id, language), ct) is not null)
                .WithMessage((_, language) => T["NotificationTypeTemplate with {0} Language already exist!", language]);
        }
    }

    public class CreateNotificationTypeTemplateRequestHandler : IRequestHandler<CreateNotificationTypeTemplateRequest, Guid>
    {
        private readonly IReadRepository<NotificationType> _notificationTypeRepo;
        private readonly IRepository<NotificationTypeTemplate> _templateRepo;
        private readonly IStringLocalizer<CreateNotificationTypeTemplateRequestHandler> _t;

        public CreateNotificationTypeTemplateRequestHandler(IReadRepository<NotificationType> notificationTypeRepo,
            IRepository<NotificationTypeTemplate> templateRepo,
            IStringLocalizer<CreateNotificationTypeTemplateRequestHandler> t)
        {
            _notificationTypeRepo = notificationTypeRepo;
            _templateRepo = templateRepo;
            _t = t;
        }

        public async Task<Guid> Handle(CreateNotificationTypeTemplateRequest request, CancellationToken cancellationToken)
        {
            var notificationType = await _notificationTypeRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = notificationType ?? throw new NotFoundException(_t["NotificationType {0} Not Found!", request.Id]);

            NotificationTypeTemplate template = new(request.Content, request.Language, false, notificationType.Id);
            await _templateRepo.AddAsync(template, cancellationToken);

            return template.Id;
        }
    }
}
