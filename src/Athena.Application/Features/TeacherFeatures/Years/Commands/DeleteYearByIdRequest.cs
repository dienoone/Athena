namespace Athena.Application.Features.TeacherFeatures.Years.Commands
{
    //ToDo: Delete EveryThing Related To the Year class here.
    public record DeleteYearByIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteYearByIdRequestHandler : IRequestHandler<DeleteYearByIdRequest, Guid>
    {
        private readonly IStringLocalizer<DeleteYearByIdRequestHandler> _t;
        private readonly IRepository<Year> _yearRepo;

        public DeleteYearByIdRequestHandler(IStringLocalizer<DeleteYearByIdRequestHandler> t, IRepository<Year> yearRepo) =>
            (_t, _yearRepo) = (t, yearRepo);

        public async Task<Guid> Handle(DeleteYearByIdRequest request, CancellationToken cancellationToken)
        {
            var year = await _yearRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = year ?? throw new NotFoundException(_t["Year {0} Not Found!", request.Id]);

            await _yearRepo.DeleteAsync(year, cancellationToken);
            return request.Id;
        }
    }
}
