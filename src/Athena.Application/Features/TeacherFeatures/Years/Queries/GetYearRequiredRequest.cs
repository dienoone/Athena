using Athena.Application.Features.DashboardFeatures.Teachers.Dto;
using Athena.Application.Features.DashboardFeatures.Teachers.Spec;

namespace Athena.Application.Features.TeacherFeatures.Years.Queries
{
    public record GetYearRequiredRequest : IRequest<List<TeacherCourseLevelDto>>;
    public class GetYearRequiredRequestHandler : IRequestHandler<GetYearRequiredRequest, List<TeacherCourseLevelDto>>
    {
        private readonly ICurrentUser _currentuser;
        private readonly IReadRepository<TeacherCourseLevel> _teacherCourseLevelRepo;
        public GetYearRequiredRequestHandler(ICurrentUser currentuser, IReadRepository<TeacherCourseLevel> teacherCourseLevelRepo) =>
            (_currentuser, _teacherCourseLevelRepo) = (currentuser, teacherCourseLevelRepo);

        public async Task<List<TeacherCourseLevelDto>> Handle(GetYearRequiredRequest request, CancellationToken cancellationToken)
        {
            var teacherCourseLevels = await _teacherCourseLevelRepo.ListAsync(new TeacherCourseLevelByBusinessIdSpec(_currentuser.GetBusinessId()), cancellationToken);
            return teacherCourseLevels.Adapt<List<TeacherCourseLevelDto>>();
        }
    }
}
