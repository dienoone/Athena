namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Commands
{
    public record DeleteHeadQuarterRequest(Guid Id) : IRequest<Guid>;

    public class DeleteHeadQuarterRequestHandler : IRequestHandler<DeleteHeadQuarterRequest, Guid>
    {
        private readonly IRepository<HeadQuarter> _repo;
        private readonly IStringLocalizer<DeleteHeadQuarterRequestHandler> _t;
        public DeleteHeadQuarterRequestHandler(IRepository<HeadQuarter> repo, IStringLocalizer<DeleteHeadQuarterRequestHandler> t) =>
            (_t, _repo) = (t, repo);

        public async Task<Guid> Handle(DeleteHeadQuarterRequest request, CancellationToken cancellationToken)
        {
            var headQuarter = await _repo.GetByIdAsync(request.Id, cancellationToken);
            _ = headQuarter ?? throw new NotFoundException(_t["HeadQuarter {0} Not Found!", request.Id]);

            await _repo.DeleteAsync(headQuarter, cancellationToken);
            return headQuarter.Id;
        }
    }
}
