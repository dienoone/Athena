using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Years.Commands
{
    // switch preopen to open
    // Check if state is preopen this no end -> raise error
    public record EndYearRequest(Guid Id) : IRequest<Guid>;

    public class EndYearRequestHandler : IRequestHandler<EndYearRequest, Guid>
    {
        private readonly IRepository<Year> _yearRepo;
        private readonly IRepository<TeacherCourseLevelYearSemster> _semsterRepo;
        private readonly IStringLocalizer<EndYearRequest> _t;

        public EndYearRequestHandler(IRepository<Year> yearRepo, IRepository<TeacherCourseLevelYearSemster> semsterRepo,
            IStringLocalizer<EndYearRequest> t) =>
            (_yearRepo, _semsterRepo, _t) = (yearRepo, semsterRepo, t);

        public async Task<Guid> Handle(EndYearRequest request, CancellationToken cancellationToken)
        {
            var year = await _yearRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = year ?? throw new NotFoundException(_t["Year {0} Not Found!", request.Id]);

            year.Update(YearStatus.Finished, false);

            await _yearRepo.UpdateAsync(year, cancellationToken);
            return request.Id;
        }
    }


}
