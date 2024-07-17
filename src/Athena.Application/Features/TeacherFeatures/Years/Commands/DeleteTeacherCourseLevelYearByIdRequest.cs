namespace Athena.Application.Features.TeacherFeatures.Years.Commands
{
    // ToDo: Delete Semster Also
    public record DeleteTeacherCourseLevelYearByIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteTeacherCourseLevelYearByIdRequestHandler : IRequestHandler<DeleteTeacherCourseLevelYearByIdRequest, Guid>
    {
        private readonly IRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;
        private readonly IStringLocalizer<DeleteTeacherCourseLevelYearByIdRequestHandler> _t;

        public DeleteTeacherCourseLevelYearByIdRequestHandler(IRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo,
            IStringLocalizer<DeleteTeacherCourseLevelYearByIdRequestHandler> t)
        {
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
            _t = t;
        }

        public async Task<Guid> Handle(DeleteTeacherCourseLevelYearByIdRequest request, CancellationToken cancellationToken)
        {
            var teacherCourseLevelYear = await _teacherCourseLevelYearRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = teacherCourseLevelYear ?? throw new NotFoundException(_t["TeacherCourseLevelYear {0} Not Found!", request.Id]);
            await _teacherCourseLevelYearRepo.DeleteAsync(teacherCourseLevelYear, cancellationToken);
            return request.Id;
        }
    }
}
