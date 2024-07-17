namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Delete
{
    public record DeleteExamGroupReqeust(Guid Id) : IRequest<Guid>;

    public class DeleteExamGroupReqeustHandler : IRequestHandler<DeleteExamGroupReqeust, Guid>
    {
        private readonly IRepository<ExamGroup> _examGroupRepo;
        private readonly IStringLocalizer<DeleteExamGroupReqeustHandler> _t;

        public DeleteExamGroupReqeustHandler(IRepository<ExamGroup> examGroupRepo, IStringLocalizer<DeleteExamGroupReqeustHandler> t)
        {
            _examGroupRepo = examGroupRepo;
            _t = t;
        }

        public async Task<Guid> Handle(DeleteExamGroupReqeust request, CancellationToken cancellationToken)
        {

            var examGroup = await _examGroupRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = examGroup ?? throw new NotFoundException(_t["ExamGroup {0} Not Found!", request.Id]);

            await _examGroupRepo.DeleteAsync(examGroup, cancellationToken);
            return examGroup.Id;
        }
    }
}
