using Athena.Application.Features.StudentFeatures.Teachers.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Teachers.Queries
{
    public record GetExploreTeacherYearForJoinByTeacherIdRequest(Guid Id) : IRequest<ExploreTeacherYearForJoinDto>;
    public class GetExploreTeacherYearForJoinByTeacherIdRequestHandler : IRequestHandler<GetExploreTeacherYearForJoinByTeacherIdRequest, ExploreTeacherYearForJoinDto>
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IReadRepository<Group> _groupRepo;
        private readonly IReadRepository<GroupStudent> _groupStudentRepo;
        private readonly IReadRepository<TeacherCourseLevelYear> _teacherCouresLevelYearRepo;
        private readonly IStringLocalizer<GetExploreTeacherDetailRequestHandler> _t;

        public GetExploreTeacherYearForJoinByTeacherIdRequestHandler(
            IUserService userService, 
            ICurrentUser currentUser, 
            IReadRepository<Teacher> teacherRepo, 
            IReadRepository<Student> studentRepo, 
            IReadRepository<Group> groupRepo, 
            IReadRepository<GroupStudent> groupStudentRepo, 
            IReadRepository<TeacherCourseLevelYear> teacherCouresLevelYearRepo, 
            IStringLocalizer<GetExploreTeacherDetailRequestHandler> t)
        {
            _userService = userService;
            _currentUser = currentUser;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _groupRepo = groupRepo;
            _groupStudentRepo = groupStudentRepo;
            _teacherCouresLevelYearRepo = teacherCouresLevelYearRepo;
            _t = t;
        }

        public async Task<ExploreTeacherYearForJoinDto> Handle(GetExploreTeacherYearForJoinByTeacherIdRequest request, CancellationToken cancellationToken)
        {
            var teacher = await GetTeacherAsync(request.Id, cancellationToken);
            var student = await GetStudentAsync(cancellationToken);
            var teacherUser = await _userService.GetAsync(teacher.Id.ToString(), cancellationToken);

            var openYearDto = await GetExploreTeacherYearDtoAsync(student.LevelClassification.LevelId, YearStatus.Open, teacher.BusinessId, cancellationToken);
            var preOpenYearDto = await GetExploreTeacherYearDtoAsync(student.LevelClassification.LevelId, YearStatus.Preopen, teacher.BusinessId, cancellationToken);

            return new() 
            { 
                TeacherId = teacher.Id,
                TeacherName = teacherUser.FirstName + " " + teacherUser.LastName,
                TeacherImage = teacher.ImagePath,
                Course = teacher.Course.Name,
                Open = openYearDto,
                Preopen = preOpenYearDto
            };
        }


        private async Task<Teacher> GetTeacherAsync(Guid teacherId, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepo.GetBySpecAsync(new TeacherByTeacherIdIncludeDetailsSpec(teacherId), cancellationToken);
            _ = teacher ?? throw new NotFoundException(_t["Teacher {0} Not Found!", teacherId]);
            return teacher;
        }

        private async Task<Student> GetStudentAsync(CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetBySpecAsync(new GetLevelByStudentIdSpec(_currentUser.GetUserId()), cancellationToken);
            _ = student ?? throw new NotFoundException(_t["Student Not Found!"]);
            return student;
        }

        public async Task<ExploreTeacherYearDto?> GetExploreTeacherYearDtoAsync(Guid levelId, string yearStatus, Guid busniessId, CancellationToken cancellationToken)
        {
            var year = await _teacherCouresLevelYearRepo.GetBySpecAsync(new TeacherYearsByLevelClassificationIdAndYearStateSpec(levelId, yearStatus, busniessId), cancellationToken);
            if (year == null) return null;

            ExploreTeacherYearDto yearDto = new() 
            { 
                Id = year.Id,
                YearState = yearStatus,
                Start = year.Year.DashboardYear.Start,
                End = year.Year.DashboardYear.Start + 1,
                IntroFee = year.IntroFee,
                MonthFee = year.MonthFee
            };
            var groups = await _groupRepo.ListAsync(new GroupsByTeacherCourseLevelYearIdIncludeHeadQuarterDetailsSpec(year.Id), cancellationToken);

            if (groups != null)
                yearDto.Groups = await GetExploreTeacherYearGroupDtosAsync(groups, cancellationToken);

            return yearDto;
        }

        public async Task<List<ExploreTeacherYearGroupDto>> GetExploreTeacherYearGroupDtosAsync(List<Group> groups, CancellationToken cancellationToken)
        {
            var groupDtos = new List<ExploreTeacherYearGroupDto>();

            foreach (var group in groups)
            {
                var groupDto = group.Adapt<ExploreTeacherYearGroupDto>();
                var groupStudents = await _groupStudentRepo.ListAsync(new GroupStudentsByGroupIdSpec(group.Id), cancellationToken);

                groupDto.Remainder = groupStudents != null ? group.Limit - groupStudents.Count : group.Limit;
                groupDtos.Add(groupDto);
            }

            return groupDtos;
        }
    }
}
