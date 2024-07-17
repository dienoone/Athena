using Athena.Application.Features.TeacherFeatures.Groups.Dtos;
using Athena.Application.Features.TeacherFeatures.HeadQuarters.Dtos;
using Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec;
using Athena.Domain.Common.Const;
using Athena.Domain.Entities;

namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Queries
{
    // ToDo: Add Groups For this request:
    public record GetHeadQuarterDetailByIdRequest(Guid Id) : IRequest<HeadQuarterDetailDto>;
    public class GetHeadQuarterDetailByIdRequestHandler : IRequestHandler<GetHeadQuarterDetailByIdRequest, HeadQuarterDetailDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<HeadQuarter> _headRepo;
        private readonly IReadRepository<Year> _yearRepo;
        private readonly IReadRepository<TeacherCourseLevel> _teacherCourseLevelRepo;
        private readonly IReadRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;
        private readonly IReadRepository<Group> _groupRepo;

        public GetHeadQuarterDetailByIdRequestHandler(
            ICurrentUser currentUser, 
            IReadRepository<HeadQuarter> headRepo, 
            IReadRepository<Year> yearRepo, 
            IReadRepository<TeacherCourseLevel> teacherCourseLevelRepo, 
            IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo, 
            IReadRepository<Group> groupRepo)
        {
            _currentUser = currentUser;
            _headRepo = headRepo;
            _yearRepo = yearRepo;
            _teacherCourseLevelRepo = teacherCourseLevelRepo;
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
            _groupRepo = groupRepo;
        }

        public async Task<HeadQuarterDetailDto> Handle(GetHeadQuarterDetailByIdRequest request, CancellationToken cancellationToken)
        {
            var headQuarter = await _headRepo.GetBySpecAsync(new HeadQuarterDetailByIdIncPhonesSpec(request.Id), cancellationToken);


            var openYear = await _yearRepo.GetBySpecAsync(new OpenYearIncludeLevelsByHeadQuarterIdAndStateSpec(request.Id, YearStatus.Open), cancellationToken);
            var preOpenYear = await _yearRepo.GetBySpecAsync(new OpenYearIncludeLevelsByHeadQuarterIdAndStateSpec(request.Id, YearStatus.Preopen), cancellationToken);


            HeadQuarterDetailDto headDto = headQuarter!.Adapt<HeadQuarterDetailDto>();
            headDto.Open = await MapYearDto(openYear, cancellationToken);
            headDto.Preopen = await MapYearDto(preOpenYear, cancellationToken);

            return headDto;
        }

        private async Task<List<HeadQuarterLevelsDto>?> MapYearDto(Year? year, CancellationToken cancellationToken)
        {
            if(year == null)
                return null;

            if (!year.TeacherCourseLevelYears.Any())
                return null;

            List<HeadQuarterLevelsDto> levelDtos = new();
            foreach(var teacherCourseLevelYear in year.TeacherCourseLevelYears)
            {
                levelDtos.Add(await MapLevelDto(teacherCourseLevelYear, cancellationToken));
            }

            return levelDtos;
        }

        private async Task<HeadQuarterLevelsDto> MapLevelDto(TeacherCourseLevelYear teacherCourseLevelYear, CancellationToken cancellationToken)
        {
            HeadQuarterLevelsDto dto = new()
            {
                Id = teacherCourseLevelYear.TeacherCourseLevel.LevelId,
                LevelName = teacherCourseLevelYear.TeacherCourseLevel.Level.Name
            };

            var groups = await GetGroupsAsync(teacherCourseLevelYear.Id, cancellationToken);

            if (groups.Any())
            {
                List<GroupsRequiredDto> groupDtos = new();
                foreach (var group in groups)
                {
                    groupDtos.Add(MapGroupDto(group));
                }
                dto.Groups = groupDtos;
            }

            return dto;
        }

        private static GroupsRequiredDto MapGroupDto(Group group)
        {
            return new()
            {
                Id = group.Id,
                Name = group.Name
            };
        }

        private async Task<IEnumerable<Group>> GetGroupsAsync(Guid teacherCouresLevelYearId, CancellationToken cancellationToken)
        {
            return await _groupRepo.ListAsync(new GroupsByTeacherCourseLevelYearIdSpec(teacherCouresLevelYearId), cancellationToken);
        }
    }
}
