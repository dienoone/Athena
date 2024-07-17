using Athena.Application.Features.DashboardFeatures.Teachers.Dto;
using Athena.Application.Features.DashboardFeatures.Teachers.Spec;
using Athena.Application.Identity.Users;

namespace Athena.Application.Features.DashboardFeatures.Teachers.Queries
{
    public record GetTeacherBaseRequest() : IRequest<TeacherBaseDto>;

    public class GetTeacherBaseRequestHandler : IRequestHandler<GetTeacherBaseRequest, TeacherBaseDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IReadRepository<Teacher> _repo;

        public GetTeacherBaseRequestHandler(ICurrentUser currentUser, IUserService userService, IReadRepository<Teacher> repo) =>
            (_currentUser, _userService, _repo) = (currentUser, userService, repo);

        public async Task<TeacherBaseDto> Handle(GetTeacherBaseRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            Console.WriteLine(userId);
            var userDetail = await _userService.GetAsync(userId.ToString(), cancellationToken);
            var userRoles = await _userService.GetRolesAsync(userId.ToString(), cancellationToken);
            var teacher = await _repo.GetBySpecAsync(new TeacherBaseByIdSpec(userId), cancellationToken);

            return new TeacherBaseDto
            {
                UserName = userDetail.UserName,
                FirstName = userDetail.FirstName,
                MiddleName = userDetail.MiddleName,
                LastName = userDetail.LastName,
                Gender = userDetail.Gender,
                Email = userDetail.Email,
                IsActive = userDetail.IsActive,
                EmailConfirmed = userDetail.EmailConfirmed,
                PhoneNumber = userDetail.PhoneNumber,
                PhoneNumberConfirmed = userDetail.PhoneNumberConfirmed,
                Address = teacher!.Address,
                ImagePath = teacher.ImagePath,
                CourseName = teacher.Course.Name,
                Roles = userRoles
            };

        }
    }

}
