namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Create
{
    public record CreateExamGroupsRequest(Guid Id, List<Guid> GroupIds) : IRequest<Guid>;

    public class CreateExamGroupsRequestHandler : IRequestHandler<CreateExamGroupsRequest, Guid>
    {
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IRepository<ExamGroup> _groupRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IStringLocalizer<CreateExamGroupsRequestHandler> _t;

        public CreateExamGroupsRequestHandler(IReadRepository<Exam> examRepo, IRepository<ExamGroup> groupRepo,
            ICurrentUser currentUser, IStringLocalizer<CreateExamGroupsRequestHandler> t)
        {
            _examRepo = examRepo;
            _groupRepo = groupRepo;
            _currentUser = currentUser;
            _t = t;
        }

        public async Task<Guid> Handle(CreateExamGroupsRequest request, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", request.Id]);

            var businessId = _currentUser.GetBusinessId();

            foreach (Guid id in request.GroupIds)
            {
                var examGroup = new ExamGroup(id, exam.Id, false, false, businessId);
                await _groupRepo.AddAsync(examGroup, cancellationToken);
            }
            return exam.Id;
        }
    }
}
