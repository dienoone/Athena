namespace Athena.Application.Features.TeacherFeatures.Groups.Commands
{
    public record CreateGroupScadual(string Day, TimeSpan StartTime, TimeSpan EndTime);

    public class CreateGroupRequest : IRequest<Guid>
    {
        public string Name { get; set; } = default!;
        public Guid HeadQuarterId { get; set; }
        public Guid TeacherCourseLevelYearId { get; set; }
        public int Limit { get; set; }
        public List<CreateGroupScadual> GroupScaduals { get; set; } = null!;

    }

    public class CreateGroupRequestHandler : IRequestHandler<CreateGroupRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Group> _groupRepo;
        private readonly IRepository<GroupScadual> _groupScadualRepo;
        public CreateGroupRequestHandler(ICurrentUser currentUser, IRepository<Group> groupRepo, IRepository<GroupScadual> groupScadualRepo) =>
            (_currentUser, _groupRepo, _groupScadualRepo) = (currentUser, groupRepo, groupScadualRepo);

        public async Task<Guid> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
        {
            Group group = new(request.Name, request.HeadQuarterId, request.TeacherCourseLevelYearId, request.Limit, _currentUser.GetBusinessId());
            await _groupRepo.AddAsync(group, cancellationToken);

            foreach (var groupScadual in request.GroupScaduals)
            {
                GroupScadual scadual = new(groupScadual.Day, groupScadual.StartTime, groupScadual.EndTime, null, group.Id, _currentUser.GetBusinessId());
                await _groupScadualRepo.AddAsync(scadual, cancellationToken);
            }

            return group.Id;
        }
    }
}
