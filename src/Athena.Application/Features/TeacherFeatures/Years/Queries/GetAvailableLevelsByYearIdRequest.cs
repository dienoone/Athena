using Athena.Application.Features.TeacherFeatures.Years.Dtos;
using Athena.Application.Features.TeacherFeatures.Years.Spec;

namespace Athena.Application.Features.TeacherFeatures.Years.Queries
{
    public record GetAvailableLevelsByYearIdRequest(Guid Id) : IRequest<List<AvailableLevelsForYearDto>>;

    public class GetAvailableLevelsByYearIdRequestHandler : IRequestHandler<GetAvailableLevelsByYearIdRequest, List<AvailableLevelsForYearDto>>
    {
        private readonly IReadRepository<TeacherCourseLevel> _teacherCourseLevelRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Year> _yearRepo;
        private readonly IStringLocalizer<GetAvailableLevelsByYearIdRequestHandler> _t;

        public GetAvailableLevelsByYearIdRequestHandler(IReadRepository<TeacherCourseLevel> teacherCourseLevelRepo,
            IStringLocalizer<GetAvailableLevelsByYearIdRequestHandler> t,
            IReadRepository<Year> yearRepo,
            ICurrentUser currentUser)
        {
            _teacherCourseLevelRepo = teacherCourseLevelRepo;
            _t = t;
            _yearRepo = yearRepo;
            _currentUser = currentUser;
        }
        public async Task<List<AvailableLevelsForYearDto>> Handle(GetAvailableLevelsByYearIdRequest request, CancellationToken cancellationToken)
        {
            var year = await _yearRepo.GetBySpecAsync(new LevelsByYearIdSpec(request.Id), cancellationToken);
            _ = year ?? throw new NotFoundException(_t["Year {0} Not Found!", request.Id]);

            List<Guid> levelIds = year.TeacherCourseLevelYears.Select(e => e.TeacherCourseLevelId).ToList();

            var levels = await _teacherCourseLevelRepo.ListAsync(new GetAvailableLevelsForYearSpec(levelIds, _currentUser.GetBusinessId()), cancellationToken);

            List<AvailableLevelsForYearDto> levelDtos = new();
            foreach(var level in levels)
            {
                AvailableLevelsForYearDto levelDto = new() 
                { 
                    TeacherCourseLevelId = level.Id,
                    LevelName = level.Level.Name,
                };
                levelDtos.Add(levelDto);
            }

            return levelDtos;
        }
    }
}
