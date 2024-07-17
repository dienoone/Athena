using Athena.Application.Features.TeacherFeatures.Groups.Dtos;
using Athena.Application.Features.TeacherFeatures.Groups.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Groups.Queries
{
    public record GetGroupListRequest() : IRequest<GroupListRequestDto>;
    public class GetGroupListRequestHandler : IRequestHandler<GetGroupListRequest, GroupListRequestDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Group> _repo;
        public GetGroupListRequestHandler(ICurrentUser currentUser, IReadRepository<Group> repo) =>
            (_currentUser, _repo) = (currentUser, repo);

        public async Task<GroupListRequestDto> Handle(GetGroupListRequest request, CancellationToken cancellationToken)
        {
            var openGroups = await _repo.ListAsync(new GroupListSpec(_currentUser.GetBusinessId(), YearStatus.Open), cancellationToken);
            var preOpenGroups = await _repo.ListAsync(new GroupListSpec(_currentUser.GetBusinessId(), YearStatus.Preopen), cancellationToken);

            return new() 
            { 
                Open = openGroups?.Adapt<List<GroupListDto>>(),
                PreOpen = preOpenGroups?.Adapt<List<GroupListDto>>()
            };

        }
    }


}
