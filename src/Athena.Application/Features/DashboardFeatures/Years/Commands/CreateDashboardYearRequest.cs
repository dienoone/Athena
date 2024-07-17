using Athena.Application.Features.DashboardFeatures.Years.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.DashboardFeatures.Years.Commands
{
    public record CreateDashboardYearRequest(int Start, string State) : IRequest<Guid>;

    // make sure that start not duplicated:
    // make sure that state is true:
    public class CreateDashboardYearRequestValidator : CustomValidator<CreateDashboardYearRequest> 
    {
        public CreateDashboardYearRequestValidator(IReadRepository<DashboardYear> yearRepo, IStringLocalizer<CreateDashboardYearRequestValidator> T)
        {
            RuleFor(e => e.Start)
                .GreaterThan(2020)
                .MustAsync(async (request, start, ct) => await yearRepo.GetBySpecAsync(new DashboardYearByStartAndStateSpec(start, request.State), ct) is null)
                .WithMessage((request) => T["DashboardYear with start {0} And State with {1} Already Exists.", request.Start, request.State]);

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

    public class CreateDashboardYearRequestHandler : IRequestHandler<CreateDashboardYearRequest, Guid>
    {
        private readonly IRepository<DashboardYear> _yearRepo;

        public CreateDashboardYearRequestHandler(IRepository<DashboardYear> yearRepo)
        {
            _yearRepo = yearRepo;
        }

        public async Task<Guid> Handle(CreateDashboardYearRequest request, CancellationToken cancellationToken)
        {
            var newYear = new DashboardYear(request.Start, null, DateTime.UtcNow, request.State, false);
            await _yearRepo.AddAsync(newYear, cancellationToken);
            return newYear.Id;
        }
    }



}
