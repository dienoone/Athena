using Athena.Application.Features.DashboardFeatures.Classification.Spec;

namespace Athena.Application.Features.DashboardFeatures.Classification.Commands
{
    public record CreateClassificationRequest(string Name) : IRequest<Guid>;

    public class CreateClassificationRequestValidator : CustomValidator<CreateClassificationRequest>
    {
        public CreateClassificationRequestValidator(IReadRepository<EducationClassification> repository, IStringLocalizer<CreateClassificationRequest> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new ClassificationByNameSpec(name), ct) is null)
                .WithMessage((_, name) => T["Classification {0} already exist.", name]);
        }
    }

    public class CreateClassificationRequestHandle : IRequestHandler<CreateClassificationRequest, Guid>
    {
        private readonly IRepository<EducationClassification> _repository;
        public CreateClassificationRequestHandle(IRepository<EducationClassification> repository) => _repository = repository;

        public async Task<Guid> Handle(CreateClassificationRequest request, CancellationToken cancellationToken)
        {
            EducationClassification classification = new(request.Name);
            await _repository.AddAsync(classification, cancellationToken);
            return classification.Id;
        }
    }

}
