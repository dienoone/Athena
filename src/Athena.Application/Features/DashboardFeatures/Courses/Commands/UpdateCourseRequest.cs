using Athena.Application.Features.DashboardFeatures.Courses.Spec;

namespace Athena.Application.Features.DashboardFeatures.Courses.Commands
{
    public record UpdateCourseRequest(Guid Id, string Name) : IRequest<Guid>;

    public class UpdateCourseRequestValidator : CustomValidator<UpdateCourseRequest>
    {
        public UpdateCourseRequestValidator(IRepository<Course> repository, IStringLocalizer<UpdateCourseRequestValidator> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(75)
                .MustAsync(async (course, name, ct) =>
                    await repository.GetBySpecAsync(new CourseByNameSpec(name), ct)
                        is not Course existingCourse || existingCourse.Id == course.Id)
                .WithMessage((_, name) => T["Course {0} already Exists {1}.", name, _.Id]);

        }
    }

    public class UpdateCourseRequestHandler : IRequestHandler<UpdateCourseRequest, Guid>
    {
        // Add Domain Events automatically by using IRepositoryWithEvents
        private readonly IRepository<Course> _repository;
        private readonly IStringLocalizer _t;

        public UpdateCourseRequestHandler(IRepository<Course> repository, IStringLocalizer<UpdateCourseRequestHandler> localizer) =>
           (_repository, _t) = (repository, localizer);

        public async Task<Guid> Handle(UpdateCourseRequest request, CancellationToken cancellationToken)
        {
            var course = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _ = course ?? throw new NotFoundException(_t["Course {0} Not Found.", request.Id]);

            course.Update(request.Name);

            await _repository.UpdateAsync(course, cancellationToken);

            return request.Id;
        }

    }


}
