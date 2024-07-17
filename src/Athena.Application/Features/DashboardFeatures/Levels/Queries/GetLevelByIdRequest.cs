using Athena.Application.Features.DashboardFeatures.Levels.Dtos;

namespace Athena.Application.Features.DashboardFeatures.Levels.Queries
{
    public record GetLevelByIdRequest(Guid Id) : IRequest<LevelDetailDto>;

    public class LevelByIdSpec : Specification<Level, LevelDetailDto>, ISingleResultSpecification
    {
        public LevelByIdSpec(Guid id) =>
            Query.Where(e => e.Id == id);

    }

    public class GetLevelByIdRequestHandler : IRequestHandler<GetLevelByIdRequest, LevelDetailDto>
    {
        private readonly IRepository<Level> _repository;
        private readonly IStringLocalizer _t;

        public GetLevelByIdRequestHandler(IRepository<Level> repository, IStringLocalizer<GetLevelByIdRequestHandler> t) =>
            (_repository, _t) = (repository, t);

        public async Task<LevelDetailDto> Handle(GetLevelByIdRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetBySpecAsync(
                (ISpecification<Level, LevelDetailDto>)new LevelByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException(_t["Level {0} Not Found.", request.Id]);
        }
    }


}
