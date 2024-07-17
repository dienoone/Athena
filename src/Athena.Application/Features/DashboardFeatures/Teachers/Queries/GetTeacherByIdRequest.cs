using Athena.Application.Features.DashboardFeatures.Teachers.Dto;
using Athena.Application.Features.DashboardFeatures.Teachers.Spec;
using Athena.Application.Identity.Users;

namespace Athena.Application.Features.DashboardFeatures.Teachers.Queries
{
    public record GetTeacherByIdRequest(Guid Id) : IRequest<TeacherBaseDto>;

    public class GetTeacherByIdRequestHandler : IRequestHandler<GetTeacherByIdRequest, TeacherBaseDto>
    {
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<GetTeacherByIdRequestHandler> _t;

        public GetTeacherByIdRequestHandler(IReadRepository<Teacher> teacherRepo, IUserService userService, IStringLocalizer<GetTeacherByIdRequestHandler> t) =>
            (_teacherRepo, _userService, _t) = (teacherRepo, userService, t);

        public async Task<TeacherBaseDto> Handle(GetTeacherByIdRequest request, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepo.GetBySpecAsync(new TeacherBaseByIdSpec(request.Id), cancellationToken);
            _ = teacher ?? throw new NotFoundException(_t["Teacher {0} Not Found", request.Id]);

            var dto = teacher.Adapt<TeacherBaseDto>();
            return dto;
        }
    }
}
