using Athena.Application.Features.DashboardFeatures.Levels.Spec;

namespace Athena.Application.Features.DashboardFeatures.Levels.Commands
{
    public record UpdateLevelRequest(Guid Id, string Name, int Index) : IRequest<Guid>;

    public class UpdateLevelRequestValidator : CustomValidator<UpdateLevelRequest>
    {
        public UpdateLevelRequestValidator(IReadRepository<Level> repository, IStringLocalizer<UpdateLevelRequestValidator> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (level, name, ct) =>
                    await repository.GetBySpecAsync(new LevelByNameSpec(name), ct)
                        is not Level existingLevel || existingLevel.Id == level.Id)
                .WithMessage((_, name) => T["Level {0} already exist.", name]);
        }
    }

    public class UpdateLevelRequestHandler : IRequestHandler<UpdateLevelRequest, Guid>
    {
        private readonly IRepository<Level> _repository;
        private readonly IStringLocalizer _t;

        public UpdateLevelRequestHandler(IRepository<Level> repository, IStringLocalizer<UpdateLevelRequestHandler> t) =>
            (_repository, _t) = (repository, t);

        public async Task<Guid> Handle(UpdateLevelRequest request, CancellationToken cancellationToken)
        {
            var level = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _ = level ?? throw new NotFoundException(_t["Level {0} doesn't exist.", request.Id]);

            level.Update(request.Name, request.Index);

            await _repository.UpdateAsync(level, cancellationToken);

            return level.Id;
        }
    }
}
