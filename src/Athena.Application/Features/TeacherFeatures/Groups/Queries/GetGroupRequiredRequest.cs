using Athena.Application.Features.TeacherFeatures.Groups.Dtos;
using Athena.Application.Features.TeacherFeatures.HeadQuarters.Dtos;
using Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec;
using Athena.Application.Features.TeacherFeatures.Years.Dtos;
using Athena.Application.Features.TeacherFeatures.Years.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Groups.Queries
{
    public record GetGroupRequiredRequest() : IRequest<GroupRequiredRequestDto>;

    public class GetGroupRequiredRequestHandler : IRequestHandler<GetGroupRequiredRequest, GroupRequiredRequestDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<HeadQuarter> _headRepo;
        private readonly IReadRepository<TeacherCourseLevelYear> _yearRepo;

        public GetGroupRequiredRequestHandler(ICurrentUser currentUser, IReadRepository<HeadQuarter> headRepo, IReadRepository<TeacherCourseLevelYear> yearRepo)
        {
            _currentUser = currentUser;
            _headRepo = headRepo;
            _yearRepo = yearRepo;
        }

        public async Task<GroupRequiredRequestDto> Handle(GetGroupRequiredRequest request, CancellationToken cancellationToken)
        {
            var headQuarters = await _headRepo.ListAsync(new HeadQuarterRequiredByBusinessIdSpec(_currentUser.GetBusinessId()), cancellationToken);
            var openYearLevels = await _yearRepo.ListAsync(new YearLevelsRequiredByBusniessIdSpec(_currentUser.GetBusinessId(), YearStatus.Open), cancellationToken);
            var preOpenYearLevels = await _yearRepo.ListAsync(new YearLevelsRequiredByBusniessIdSpec(_currentUser.GetBusinessId(), YearStatus.Preopen), cancellationToken);

            return new GroupRequiredRequestDto
            {
                HeadQuaertes = headQuarters?.Adapt<List<HeadQuaerteRequiredDto>>(),
                Open = openYearLevels?.Adapt<List<LevelsRequiredDto>>(),
                Preopen = preOpenYearLevels?.Adapt<List<LevelsRequiredDto>>()
            };
        }
    }
}
