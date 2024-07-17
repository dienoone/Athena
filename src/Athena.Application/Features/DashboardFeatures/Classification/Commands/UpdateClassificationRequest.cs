using Athena.Application.Features.DashboardFeatures.Classification.Spec;

namespace Athena.Application.Features.DashboardFeatures.Classification.Commands
{
    public record UpdateClassificationRequest(Guid Id, string Name) : IRequest<Guid>;

    public class UpdateClassificationRequestValidator : CustomValidator<UpdateClassificationRequest>
    {
        public UpdateClassificationRequestValidator(IReadRepository<EducationClassification> repository, IStringLocalizer<UpdateClassificationRequestValidator> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (classification, name, ct) =>
                    await repository.GetBySpecAsync(new ClassificationByNameSpec(name), ct)
                        is not EducationClassification existingClassification || existingClassification.Id == classification.Id)
                .WithMessage((_, name) => T["Classification {0} already exist.", name]);
        }
    }

    public class UpdateClassificationRequestHandler : IRequestHandler<UpdateClassificationRequest, Guid>
    {
        private readonly IRepository<EducationClassification> _repository;
        private readonly IStringLocalizer<UpdateClassificationRequestHandler> _t;

        public UpdateClassificationRequestHandler(IRepository<EducationClassification> repository, IStringLocalizer<UpdateClassificationRequestHandler> t) =>
            (_repository, _t) = (repository, t);

        public async Task<Guid> Handle(UpdateClassificationRequest request, CancellationToken cancellationToken)
        {
            var classification = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _ = classification ?? throw new NotFoundException(_t["Classification {0} Not Found!", request.Id]);

            classification.Update(request.Name);

            await _repository.UpdateAsync(classification, cancellationToken);

            return request.Id;
        }
    }

}
