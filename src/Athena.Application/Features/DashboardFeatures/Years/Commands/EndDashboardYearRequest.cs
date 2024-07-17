using Athena.Domain.Common.Const;

namespace Athena.Application.Features.DashboardFeatures.Years.Commands
{
    public record EndDashboardYearRequest(Guid Id) : IRequest<Guid>;

    public class EndDashboardYearRequestHandler : IRequestHandler<EndDashboardYearRequest, Guid>
    {
        private readonly IRepository<DashboardYear> _dashboaredYearRepo;
        private readonly IStringLocalizer<EndDashboardYearRequestHandler> _t;

        public EndDashboardYearRequestHandler(IRepository<DashboardYear> dashboaredYearRepo, IStringLocalizer<EndDashboardYearRequestHandler> t)
        {
            _dashboaredYearRepo = dashboaredYearRepo;
            _t = t;
        }

        public async Task<Guid> Handle(EndDashboardYearRequest request, CancellationToken cancellationToken)
        {
            var year = await _dashboaredYearRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = year ?? throw new NotFoundException(_t["DashboardYear {0} Not Found!", request.Id]);

            year.Update(null, DateTime.UtcNow, YearStatus.Finished, true);
            await _dashboaredYearRepo.UpdateAsync(year, cancellationToken);
            return year.Id;
        }
    }
}
