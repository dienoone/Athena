using Athena.Application.Features.StudentFeatures.Teachers.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Teachers.Queries
{
    public record GetJoinRequestByRequestIdRequest(Guid Id) : IRequest<ExploreTeacherRequestReviewDto>;

    public class GetJoinRequestByRequestIdRequestHandler : IRequestHandler<GetJoinRequestByRequestIdRequest, ExploreTeacherRequestReviewDto>
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<StudentTeacherRequest> _studentTeacherRequestRepo;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IReadRepository<Group> _groupRepo;
        private readonly IReadRepository<GroupStudent> _groupStudentRepo;
        private readonly IReadRepository<TeacherCourseLevelYear> _teacherCouresLevelYearRepo;
        private readonly IStringLocalizer<GetJoinRequestByRequestIdRequestHandler> _t;

        public GetJoinRequestByRequestIdRequestHandler(
            IUserService userService,
            ICurrentUser currentUser,
            IReadRepository<StudentTeacherRequest> studentTeacherRequestRepo,
            IReadRepository<Teacher> teacherRepo,
            IReadRepository<Student> studentRepo,
            IReadRepository<Group> groupRepo,
            IReadRepository<GroupStudent> groupStudentRepo,
            IReadRepository<TeacherCourseLevelYear> teacherCouresLevelYearRepo, 
            IStringLocalizer<GetJoinRequestByRequestIdRequestHandler> t)
        {
            _userService = userService;
            _currentUser = currentUser;
            _studentTeacherRequestRepo = studentTeacherRequestRepo;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _groupRepo = groupRepo;
            _groupStudentRepo = groupStudentRepo;
            _teacherCouresLevelYearRepo = teacherCouresLevelYearRepo;
            _t = t;
        }

        public async Task<ExploreTeacherRequestReviewDto> Handle(GetJoinRequestByRequestIdRequest request, CancellationToken cancellationToken)
        {
            var studentTeacherRequest = await GetStudentTeacherRequestAsync(request.Id, cancellationToken);
            var teacher = await GetTeacherAsync(studentTeacherRequest.TeacherId, cancellationToken);
            var student = await GetStudentAsync(cancellationToken);
            var teacherUser = await _userService.GetAsync(teacher.Id.ToString(), cancellationToken);

            var openYearDto = await GetExploreTeacherYearDtoAsync(student.LevelClassification.LevelId, YearStatus.Open, teacher.BusinessId, cancellationToken);
            var preOpenYearDto = await GetExploreTeacherYearDtoAsync(student.LevelClassification.LevelId, YearStatus.Preopen, teacher.BusinessId, cancellationToken);

            StudentChoiceDto studentChoiceDto = new() 
            { 
                GroupId = studentTeacherRequest.GroupId,
                YearId = studentTeacherRequest.Group.TeacherCourseLevelYear.Id,
            };

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

        private async Task<StudentTeacherRequest> GetStudentTeacherRequestAsync(Guid requestId,  CancellationToken cancellationToken)
        {
            var request = await _studentTeacherRequestRepo.GetByIdAsync(requestId, cancellationToken);
            _ = request ?? throw new NotFoundException(_t["StudentTeacherRequest {0} Not Found!", requestId]);

            if (request.State != StudentTeacherRequestStatus.Pending)
                throw new ConflictException(_t["You can't access this request because it state is {0}", request.State]);

            return request;
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
