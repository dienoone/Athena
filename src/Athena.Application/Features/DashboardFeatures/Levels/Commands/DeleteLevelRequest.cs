namespace Athena.Application.Features.DashboardFeatures.Levels.Commands
{
    public record DeleteLevelRequest(Guid Id) : IRequest<Guid>;

    public class DeleteLevelRequestHandler : IRequestHandler<DeleteLevelRequest, Guid>
    {
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IRepository<Level> _levelRepo;
        private readonly IStringLocalizer _t;

        public DeleteLevelRequestHandler(IReadRepository<Student> studentRepo, IRepository<Level> levelRepo, IStringLocalizer<DeleteLevelRequest> t) =>
            (_studentRepo, _levelRepo, _t) = (studentRepo, levelRepo, t);

        public async Task<Guid> Handle(DeleteLevelRequest request, CancellationToken cancellationToken)
        {
            var level = await _levelRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = level ?? throw new NotFoundException(_t["Level {0} doesn't exist.", request.Id]);

            /*if(await _studentRepo.AnyAsync(new StudentsByLevelSpec(request.Id), cancellationToken))
            {
                throw new ConflictException(_t["Level cannot be deleted as it's being used."]);
            }*/

            await _levelRepo.DeleteAsync(level, cancellationToken);

            return level.Id;
        }
    }
}
