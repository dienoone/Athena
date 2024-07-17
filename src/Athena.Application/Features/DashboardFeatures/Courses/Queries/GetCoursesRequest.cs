using Athena.Application.Features.DashboardFeatures.Courses.Dtos;

namespace Athena.Application.Features.DashboardFeatures.Courses.Queries
{
    public record GetCoursesRequest() : IRequest<List<CourseDto>>;

    public class GetCoursesRequestHandler : IRequestHandler<GetCoursesRequest, List<CourseDto>>
    {
        private readonly IRepository<Course> _repository;
        private readonly ILanguageService _languageService;
        public GetCoursesRequestHandler(IRepository<Course> repository, ILanguageService languageService) =>
            (_repository, _languageService) = (repository, languageService);

        public async Task<List<CourseDto>> Handle(GetCoursesRequest request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Current Language: {_languageService.GetCurrentLanguage()}");
            var courses = await _repository.ListAsync(cancellationToken);
            return courses.Adapt<List<CourseDto>>();
        }
    }

}
