using Athena.Application.Features.TeacherFeatures.Years.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Years.Commands
{
    public record UpdateYearStateRequest(Guid Id, string State) : IRequest<Guid>;

    public class UpdateYearRequestValidator : CustomValidator<UpdateYearStateRequest>
    {
        public UpdateYearRequestValidator(ICurrentUser currentUser, IReadRepository<Year> yearRepo, IStringLocalizer<UpdateYearRequestValidator> T)
        {

            RuleFor(e => e.State)
                .Must(state => IsValidYearState(state))
                .WithMessage((_, state) => T["Invalid Year State: {0}.", state])
                .MustAsync(async (year, state, ct) =>
                    await yearRepo.GetBySpecAsync(new OpenYearByStateSpec(state, currentUser.GetBusinessId()), ct)
                        is not Year existingYear || existingYear.Id == year.Id)
                .WithMessage((_, state) => T["Year with state {0} already exsit!", state]);
        }

        // Helper method to check if the state is valid
        private static bool IsValidYearState(string state)
        {
            return state == YearStatus.Open || state == YearStatus.Preopen;
        }

    }

    public class UpdateYearRequestHandler : IRequestHandler<UpdateYearStateRequest, Guid>
    {
        private readonly IRepository<Year> _yearRepo;
        
        public UpdateYearRequestHandler(IRepository<Year> yearRepo) => _yearRepo = yearRepo;

        public async Task<Guid> Handle(UpdateYearStateRequest request, CancellationToken cancellationToken)
        {
            var year = await _yearRepo.GetByIdAsync(request.Id, cancellationToken);
            year!.Update(request.State, null);
            await _yearRepo.UpdateAsync(year, cancellationToken);
            return request.Id;
        }
    }


}
