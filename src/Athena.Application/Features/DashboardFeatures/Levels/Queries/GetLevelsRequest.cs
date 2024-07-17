using Athena.Application.Features.DashboardFeatures.Levels.Dtos;
using Athena.Application.Features.DashboardFeatures.Levels.Spec;

namespace Athena.Application.Features.DashboardFeatures.Levels.Queries
{
    public record GetLevelsRequest() : IRequest<List<LevelDetailDto>>;

    public class GetLevelsRequestHandler : IRequestHandler<GetLevelsRequest, List<LevelDetailDto>>
    {
        private readonly IRepository<Level> _repository;
        public GetLevelsRequestHandler(IRepository<Level> repository) => _repository = repository;

        public async Task<List<LevelDetailDto>> Handle(GetLevelsRequest request, CancellationToken cancellationToken)
        {
            var levels = await _repository.ListAsync(new LevelWithLevelClassififcationsSpec(), cancellationToken);
            return levels.Adapt<List<LevelDetailDto>>();
        }
    }
}
