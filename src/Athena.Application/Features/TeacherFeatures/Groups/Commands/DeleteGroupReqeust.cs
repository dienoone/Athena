namespace Athena.Application.Features.TeacherFeatures.Groups.Commands
{
    public record DeleteGroupReqeust(Guid Id) : IRequest<Guid>;

    public class DeleteGroupReqeustHandler : IRequestHandler<DeleteGroupReqeust, Guid>
    {
        private readonly IRepository<Group> _groupRepo;
        private readonly IStringLocalizer<DeleteGroupReqeustHandler> _t;

        public DeleteGroupReqeustHandler(IRepository<Group> groupRepo, IStringLocalizer<DeleteGroupReqeustHandler> t) =>
            (_groupRepo, _t) = (groupRepo, t);

        public async Task<Guid> Handle(DeleteGroupReqeust request, CancellationToken cancellationToken)
        {
            var group = await _groupRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = group ?? throw new NotFoundException(_t["Group {0} Not Found!", request.Id]);

            await _groupRepo.DeleteAsync(group, cancellationToken);
            return request.Id;
        }
    }
}
