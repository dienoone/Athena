using Athena.Application.Features.DashboardFeatures.Years.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.DashboardFeatures.Years.Commands
{
    public record UpdateDashboardYearRequest(Guid Id, int Start, string State) : IRequest<Guid>;
    
    public class UpdateDashboardYearRequestValidator : CustomValidator<UpdateDashboardYearRequest>
    {
        public UpdateDashboardYearRequestValidator(IReadRepository<DashboardYear> repository, IStringLocalizer<UpdateDashboardYearRequestValidator> T)
        {
            RuleFor(e => e.Start)
            .GreaterThan(2020)
                .MustAsync(async (year, start, ct) =>
                    await repository.GetBySpecAsync(new DashboardYearByStartAndStateSpec(start, year.State), ct)
                        is not DashboardYear existingDashboardYear || existingDashboardYear.Id == year.Id)
                .WithMessage((request, start) => T["DashboardYear With Start {0}, And State With {1} already exist.", start, request.State]); ;

            RuleFor(e => e.State)
                .NotEmpty()
                .NotNull()
                .WithMessage((_, _) => T["Year State Can't be null."])
                .Must(state => IsValidYearState(state))
                .WithMessage((_, state) => T["Invalid Year State: {0}.", state]);
        }

        // Helper method to check if the state is valid
        private static bool IsValidYearState(string state)
        {
            return state == YearStatus.Open || state == YearStatus.Preopen;
        }
    }

    public class UpdateDashboardYearRequestHandler : IRequestHandler<UpdateDashboardYearRequest, Guid>
    {
        private readonly IRepository<DashboardYear> _dashboardYearRepo;
        private readonly IStringLocalizer<UpdateDashboardYearRequestHandler> _t;

        public UpdateDashboardYearRequestHandler(IRepository<DashboardYear> dashboardYearRepo, IStringLocalizer<UpdateDashboardYearRequestHandler> t)
        {
            _dashboardYearRepo = dashboardYearRepo;
            _t = t;
        }
        public async Task<Guid> Handle(UpdateDashboardYearRequest request, CancellationToken cancellationToken)
        {
            var year = await _dashboardYearRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = year ?? throw new NotFoundException(_t["DashboardYear {0} Not Found!", request.Id]);

            year.Update(request.Start, null, request.State, null);
            await _dashboardYearRepo.UpdateAsync(year, cancellationToken);
            return year.Id;
        }
    }
}
