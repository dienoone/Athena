namespace Athena.Application.Features.DashboardFeatures.Courses.Commands
{
    // Todo: Look There When Deleting
    public record DeleteCourseRequest(Guid Id) : IRequest<Guid>;

    public class DeleteCourseRequestHandler : IRequestHandler<DeleteCourseRequest, Guid>
    {
        private readonly IRepository<Course> _courseRepo;
        private readonly IStringLocalizer _t;

        public DeleteCourseRequestHandler(IRepository<Course> courseRepo, IStringLocalizer<DeleteCourseRequestHandler> t) =>
        (_courseRepo, _t) = (courseRepo, t);

        public async Task<Guid> Handle(DeleteCourseRequest request, CancellationToken cancellationToken)
        {
            /*if (await _teacherCourseRepo.AnyAsync(new TeachersByCourseSpec(request.Id), cancellationToken))
            {
                throw new ConflictException(_t["Course cannot be deleted as it's being used."]);
            }*/

            var course = await _courseRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = course ?? throw new NotFoundException(_t["Course {0} Not Found.", request.Id]);

            await _courseRepo.DeleteAsync(course, cancellationToken);

            return course.Id;
        }
    }
}
