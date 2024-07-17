using Athena.Application.Features.DashboardFeatures.Levels.Spec;

namespace Athena.Application.Features.DashboardFeatures.Levels.Commands
{

    public class CreateLevelRequest : IRequest<Guid>
    {
        public string Name { get; set; } = null!;
        public int Index { get; set; }
        public List<Guid>? LevelClassificationIds { get; set; }

    }

    public class CreateLevelRequestValidator : CustomValidator<CreateLevelRequest>
    {
        public CreateLevelRequestValidator(IReadRepository<Level> repository, IReadRepository<EducationClassification> classificationRepostiory, IStringLocalizer<CreateLevelRequestValidator> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new LevelByNameSpec(name), ct) is null)
                .WithMessage((_, name) => T["Level {0} already exist.", name]);

            RuleFor(e => e.LevelClassificationIds)
                .MustAsync(async (_, LevelClassificationIds, ct) => await CheckClassification(LevelClassificationIds, classificationRepostiory, T, ct));
        }

        private static async Task<bool> CheckClassification(List<Guid>? classifications, IReadRepository<EducationClassification> classificationRepostiory, IStringLocalizer<CreateLevelRequestValidator> T, CancellationToken cancellationToken)
        {
            foreach (var classificationId in classifications!)
            {
                var classification = await classificationRepostiory.GetByIdAsync(classificationId, cancellationToken);
                _ = classification ?? throw new NotFoundException(T["Classification {0} Not Found!", classificationId]);
            }
            return true;
        }
    }

    public class CreateLevelRequestHandler : IRequestHandler<CreateLevelRequest, Guid>
    {
        private readonly IRepository<Level> _repository;
        private readonly IRepository<LevelClassification> _levelClassificationRepository;

        public CreateLevelRequestHandler(IRepository<Level> repository, IRepository<LevelClassification> levelClassificationRepository) =>
            (_repository, _levelClassificationRepository) = (repository, levelClassificationRepository);

        public async Task<Guid> Handle(CreateLevelRequest request, CancellationToken cancellationToken)
        {
            Level level = new(request.Name, request.Index);
            await _repository.AddAsync(level, cancellationToken);

            foreach (var classificationId in request.LevelClassificationIds!)
            {
                LevelClassification levelClassification = new(level.Id, classificationId);
                await _levelClassificationRepository.AddAsync(levelClassification, cancellationToken);
            }

            return level.Id;
        }
    }
}
