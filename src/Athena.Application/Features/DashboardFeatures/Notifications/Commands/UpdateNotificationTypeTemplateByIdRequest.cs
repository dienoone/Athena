using Athena.Domain.Common.Const;

namespace Athena.Application.Features.DashboardFeatures.Notifications.Commands
{
    public record UpdateNotificationTypeTemplateByIdRequest(Guid Id, string Content, string Language) : IRequest<Guid>;

    public class UpdateNotificationTypeTemplateByIdRequestValidator : CustomValidator<UpdateNotificationTypeTemplateByIdRequest>
    {
        public UpdateNotificationTypeTemplateByIdRequestValidator(IStringLocalizer<UpdateNotificationTypeTemplateByIdRequestValidator> T)
        {
            RuleFor(e => e.Language)
                .NotEmpty()
                .NotNull()
                .Must((_, language) => language == Languages.Arabic || language == Languages.English)
                .WithMessage((_, language) => T["Invalid Language: {0}", language]);
        }
    }

    public class UpdateNotificationTypeTemplateByIdRequestHandler : IRequestHandler<UpdateNotificationTypeTemplateByIdRequest, Guid>
    {
        private readonly IRepository<NotificationTypeTemplate> _templateRepo;
        private readonly IStringLocalizer<UpdateNotificationTypeTemplateByIdRequestHandler> _t;

        public UpdateNotificationTypeTemplateByIdRequestHandler(IRepository<NotificationTypeTemplate> templateRepo,
            IStringLocalizer<UpdateNotificationTypeTemplateByIdRequestHandler> t)
        {
            _templateRepo = templateRepo;
            _t = t;
        }

        public async Task<Guid> Handle(UpdateNotificationTypeTemplateByIdRequest request, CancellationToken cancellationToken)
        {
            var template = await _templateRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = template ?? throw new NotFoundException(_t["NotificationTypeTemplate {0} Not Found!", request.Id]);

            template.Update(template.Content, template.Language, false);
            await _templateRepo.UpdateAsync(template, cancellationToken);
            return template.Id;
        }
    }
}
