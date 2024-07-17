using Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Queries
{
    public record GetJoinRequestsRequest() : IRequest<List<JoinRequestLevelDto>>;

    public class GetStudentRequestsRequestHandler : IRequestHandler<GetJoinRequestsRequest, List<JoinRequestLevelDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IReadRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;
        private readonly IReadRepository<StudentTeacherRequest> _studentTeacherRequestRepo;

        public GetStudentRequestsRequestHandler(
            ICurrentUser currentUser, 
            IUserService userService, 
            IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo,
            IReadRepository<StudentTeacherRequest> studentTeacherRequestRepo)
        {
            _currentUser = currentUser;
            _userService = userService;
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
            _studentTeacherRequestRepo = studentTeacherRequestRepo;
        }

        public async Task<List<JoinRequestLevelDto>> Handle(GetJoinRequestsRequest request, CancellationToken cancellationToken)
        {
            var businessId = _currentUser.GetBusinessId();
            var openYears = await GetOpenTeacherCourseLevelYearsAsync(businessId, cancellationToken);

            if (openYears == null)
            {
                return new List<JoinRequestLevelDto>();
            }

            var levelDtos = new List<JoinRequestLevelDto>();

            foreach (var year in openYears)
            {
                var requests = await GetPendingStudentRequestsAsync(year.Id, businessId, cancellationToken);

                if (requests.Any())
                {
                    var dtos = await CreateJoinStudentRequestDtos(requests, year, cancellationToken);
                    var levelDto = new JoinRequestLevelDto
                    {
                        LevelName = year.TeacherCourseLevel.Level.Name,
                        Students = dtos
                    };
                    levelDtos.Add(levelDto);
                }
            }

            return levelDtos;
        }

        private async Task<List<TeacherCourseLevelYear>> GetOpenTeacherCourseLevelYearsAsync(Guid businessId, CancellationToken cancellationToken)
        {
            return await _teacherCourseLevelYearRepo.ListAsync(
                new OpenTeacherCourseLevelYearsByBusinessIdIncludeLevelAndDashboardYearSpec(businessId), cancellationToken);
        }

        private async Task<List<StudentTeacherRequest>> GetPendingStudentRequestsAsync(Guid yearId, Guid businessId, CancellationToken cancellationToken)
        {
            return await _studentTeacherRequestRepo.ListAsync(
                new StudentTeacherRequestByStateAndTeacherCouresLevelYearIdAndBusinessIdSpec(
                    StudentTeacherRequestStatus.Pending, yearId, businessId), cancellationToken);
        }

        private async Task<List<JoinStudentRequestDto>> CreateJoinStudentRequestDtos(List<StudentTeacherRequest> requests, TeacherCourseLevelYear year, CancellationToken cancellationToken)
        {
            var dtos = new List<JoinStudentRequestDto>();

            foreach (var studentRequest in requests)
            {
                var user = await _userService.GetAsync(studentRequest.StudentId.ToString(), cancellationToken);
                var dto = new JoinStudentRequestDto
                {
                    Id = studentRequest.Id,
                    Name = $"{user.FirstName} {user.MiddleName} {user.LastName}",
                    Gender = user.Gender,
                    Image = studentRequest.Student.Image,
                    GroupName = studentRequest.Group.Name,
                    YearState = year.Year.DashboardYear.State
                };
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
