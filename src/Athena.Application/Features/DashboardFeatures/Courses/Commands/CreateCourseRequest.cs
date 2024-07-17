using Athena.Application.Features.DashboardFeatures.Courses.Spec;

namespace Athena.Application.Features.DashboardFeatures.Courses.Commands
{
    public record CreateCourseRequest(string Name) : IRequest<Guid>;

    public class CreateCourseRequestValidator : CustomValidator<CreateCourseRequest>
    {
        public CreateCourseRequestValidator(IReadRepository<Course> repository, IStringLocalizer<CreateCourseRequestValidator> T)
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(75)
                .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new CourseByNameSpec(name), ct) is null)
                .WithMessage((_, name) => T["Course {0} aleardy exists.", name]);
        }
    }

    public class CreateCourseRequestHandler : IRequestHandler<CreateCourseRequest, Guid>
    {
        private readonly IRepository<Course> _repository;

        public CreateCourseRequestHandler(IRepository<Course> repository) => _repository = repository;

        public async Task<Guid> Handle(CreateCourseRequest request, CancellationToken cancellationToken)
        {
            var course = new Course(request.Name);

            await _repository.AddAsync(course, cancellationToken);

            return course.Id;
        }
    }

}
