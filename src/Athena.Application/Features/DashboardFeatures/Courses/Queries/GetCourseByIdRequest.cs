using Athena.Application.Features.DashboardFeatures.Courses.Dtos;

namespace Athena.Application.Features.DashboardFeatures.Courses.Queries
{
    public record GetCourseByIdRequest(Guid Id) : IRequest<CourseDto>;

    public class GetCourseByIdRequestHandler : IRequestHandler<GetCourseByIdRequest, CourseDto>
    {
        private readonly IRepository<Course> _repository;
        private readonly IStringLocalizer _t;

        public GetCourseByIdRequestHandler(IRepository<Course> repository, IStringLocalizer<GetCourseByIdRequestHandler> localizer) =>
            (_repository, _t) = (repository, localizer);

        public async Task<CourseDto> Handle(GetCourseByIdRequest request, CancellationToken cancellationToken)
        {
            var course = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _ = course ?? throw new NotFoundException(_t["Course {0} Not Found.", request.Id]);
            return course.Adapt<CourseDto>();
        }
    }

}
