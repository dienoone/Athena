using Athena.Application.Features.TeacherFeatures.HeadQuarters.Dtos;
using Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec;

namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Queries
{
    public record GetHeadQuartersListRequest() : IRequest<List<HeadQuarterListDto>>;

    public class GetHeadQuartersListRequestHandler : IRequestHandler<GetHeadQuartersListRequest, List<HeadQuarterListDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<HeadQuarter> _headRepo;
        public GetHeadQuartersListRequestHandler(ICurrentUser currentUser, IReadRepository<HeadQuarter> headRepo) =>
            (_currentUser, _headRepo) = (currentUser, headRepo);

        public async Task<List<HeadQuarterListDto>> Handle(GetHeadQuartersListRequest request, CancellationToken cancellationToken)
        {
            var heads = await _headRepo.ListAsync(new HeadQuarterListIncPhonesSpec(_currentUser.GetBusinessId()), cancellationToken);
            return heads.Adapt<List<HeadQuarterListDto>>();
        }
    }


}
