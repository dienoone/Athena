using Athena.Application.Features.DashboardFeatures.Years.Dtos;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.DashboardFeatures.Years.Queries
{
    public record GetYearsReqesut() : IRequest<YearsRequestDto>;

    public class GetYearsReqesutHandler : IRequestHandler<GetYearsReqesut, YearsRequestDto>
    {
        private readonly IReadRepository<DashboardYear> _yearRepo;

        public GetYearsReqesutHandler(IReadRepository<DashboardYear> yearRepo)
        {
            _yearRepo = yearRepo;
        }
        public async Task<YearsRequestDto> Handle(GetYearsReqesut request, CancellationToken cancellationToken)
        {
            var years = await _yearRepo.ListAsync(cancellationToken);

            return new() 
            {
                Open = years?.Where(e => e.State == YearStatus.Open).Adapt<List<DashboardYearDto>>(),
                Preopen = years?.Where(e => e.State == YearStatus.Preopen).Adapt<List<DashboardYearDto>>(),
                Finished = years?.Where(e => e.State == YearStatus.Finished).Adapt<List<DashboardYearDto>>()
            };
        }
    }
}
