namespace Athena.Application.Features.DashboardFeatures.ExamTypes.Commands
{
    public record DeleteExamTypeRequest(Guid Id) : IRequest<Guid>;

    public class DeleteExamTypeRequestHandler : IRequestHandler<DeleteExamTypeRequest, Guid>
    {
        private readonly IRepository<ExamType> _examTypeRepo;
        private readonly IStringLocalizer _t;

        public DeleteExamTypeRequestHandler(IRepository<ExamType> examTypeRepo, IStringLocalizer<DeleteExamTypeRequestHandler> t) =>
        (_examTypeRepo, _t) = (examTypeRepo, t);

        public async Task<Guid> Handle(DeleteExamTypeRequest request, CancellationToken cancellationToken)
        {
            /*if (await _teacherCourseRepo.AnyAsync(new TeachersByCourseSpec(request.Id), cancellationToken))
            {
                throw new ConflictException(_t["Course cannot be deleted as it's being used."]);
            }*/

            var examType = await _examTypeRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = examType ?? throw new NotFoundException(_t["ExamType {0} Not Found.", request.Id]);

            await _examTypeRepo.DeleteAsync(examType, cancellationToken);

            return examType.Id;
        }
    }
}
