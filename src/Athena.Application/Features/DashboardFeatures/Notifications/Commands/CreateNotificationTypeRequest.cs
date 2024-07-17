using Athena.Application.Features.DashboardFeatures.Notifications.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.DashboardFeatures.Notifications.Commands
{
    public record CreateNotificationTypeTemplate(string Content, string Language);
    public class CreateNotificationTypeRequest : IRequest<Guid>
    {
        public string Type { get; set; } = default!;
        public string? Description { get; set; } 
        public List<CreateNotificationTypeTemplate> Templates { get; set; } = null!;
    }

    public class CreateNotificationTypeRequestValidator : CustomValidator<CreateNotificationTypeRequest>
    {
        public CreateNotificationTypeRequestValidator(IReadRepository<NotificationType> repo, 
            IStringLocalizer<CreateNotificationTypeRequestValidator> T)
        {
            RuleFor(e => e.Type)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (type, ct) => await repo.GetBySpecAsync(new NotificationTypeByTypeSpec(type), ct) is null)
                .WithMessage((type, _) => T["NotificationType {0} already exist!", type]);

            RuleFor(e => e.Templates)
                .Must((_, templates) => CheckTemplates(templates, T));
        }

        private static bool CheckTemplates(List<CreateNotificationTypeTemplate> templates,
            IStringLocalizer<CreateNotificationTypeRequestValidator> T)
        {
            if (templates.Count != 2)
                throw new ConflictException(T["Notification must have two templates"]);

            bool hasArabic = templates.Any(template => template.Language == Languages.Arabic);
            bool hasEnglish = templates.Any(template => template.Language == Languages.English);

            if (!hasArabic || !hasEnglish)
                throw new ConflictException(T["Notification must have templates in both Arabic and English"]);

            return true;
        }

    }

    public class CreateNotificationTypeRequestHandler : IRequestHandler<CreateNotificationTypeRequest, Guid>
    {
        private readonly IRepository<NotificationType> _notificationTypeRepo;
        private readonly IRepository<NotificationTypeTemplate> _notificationTypeTemplateRepo;

        public CreateNotificationTypeRequestHandler(IRepository<NotificationType> notificationTypeRepo, 
            IRepository<NotificationTypeTemplate> notificationTypeTemplateRepo)
        {
            _notificationTypeRepo = notificationTypeRepo;
            _notificationTypeTemplateRepo = notificationTypeTemplateRepo;
        }

        public async Task<Guid> Handle(CreateNotificationTypeRequest request, CancellationToken cancellationToken)
        {
            NotificationType notificationType = new(request.Type, request.Description, false);
            await _notificationTypeRepo.AddAsync(notificationType, cancellationToken);

            foreach(var template in request.Templates)
            {
                NotificationTypeTemplate notificationTypeTemplate = new(template.Content, template.Language, false, notificationType.Id);
                await _notificationTypeTemplateRepo.AddAsync(notificationTypeTemplate, cancellationToken);
            }

            return notificationType.Id;
        }
    }
}
