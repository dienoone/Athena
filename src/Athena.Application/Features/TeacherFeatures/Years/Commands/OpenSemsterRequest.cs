/*using Athena.Application.Features.TeacherFeatures.Years.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Years.Commands
{
    public record OpenSemsterRequest(Guid Id) : IRequest<Guid>;

    public class OpenSemsterRequestHandler : IRequestHandler<OpenSemsterRequest, Guid>
    {
        private readonly IRepository<TeacherCourseLevelYearSemster> _semsterRepo;
        private readonly IStringLocalizer<OpenSemsterRequestHandler> _t;

        public OpenSemsterRequestHandler(IRepository<TeacherCourseLevelYearSemster> semsterRepo, IStringLocalizer<OpenSemsterRequestHandler> t)
        {
            _semsterRepo = semsterRepo;
            _t = t;
        }

        public async Task<Guid> Handle(OpenSemsterRequest request, CancellationToken cancellationToken)
        {
            var semster = await _semsterRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = semster ?? throw new InternalServerException(_t["Semster {0} Not Found!", request.Id]);

            if (semster.Semster.Equals(Semster.FirstSemster))
            {
                await FirstSemster(request, semster, cancellationToken);
            }
            else
            {
                await SecondSemster(request, semster, cancellationToken);
            }


            return request.Id;
        }

        private async Task FirstSemster(OpenSemsterRequest request, TeacherCourseLevelYearSemster semster,
            CancellationToken cancellationToken)
        {
            switch (semster.State)
            {
                case Semster.ActiveState:
                    throw new ConflictException(_t["Semster {0} Is Already Active", request.Id]);

                case Semster.FinishedState:
                    throw new ConflictException(_t["Semster {0} Is Finished You Can't Active It Again", request.Id]);

                case Semster.DisactiveState:

                    var secondSemster = await _semsterRepo.GetBySpecAsync(new OtherSemsterSpec(semster.TeacherCourseLevelYearId, Semster.SecondSemster), cancellationToken);

                    if (secondSemster!.State.Equals(Semster.ActiveState))
                        throw new ConflictException(_t["You Need To Finish The Second Semster First!", request.Id]);
                    else
                    {
                        semster.Update(DateTime.UtcNow, null, Semster.ActiveState);
                        await _semsterRepo.UpdateAsync(semster, cancellationToken);
                    }
                    break; // Add a break statement here

                default:
                    throw new InvalidOperationException("Invalid semester state.");
            }
        }

        private async Task SecondSemster(OpenSemsterRequest request, TeacherCourseLevelYearSemster semster,
            CancellationToken cancellationToken)
        {
            switch (semster.State)
            {
                case Semster.ActiveState:
                    throw new ConflictException(_t["Semster {0} Is Already Active", request.Id]);

                case Semster.FinishedState:
                    throw new ConflictException(_t["Semster {0} Is Finished You Can't Active It Again", request.Id]);

                case Semster.DisactiveState:

                    var firstSemster = await _semsterRepo.GetBySpecAsync(
                         new OtherSemsterSpec(semster.TeacherCourseLevelYearId, Semster.FirstSemster));

                    if (firstSemster!.State.Equals(Semster.ActiveState))
                        throw new ConflictException(_t["You Need To Finish The First Semster First!", request.Id]);
                    else
                    {
                        semster.Update(DateTime.UtcNow, null, Semster.ActiveState);
                        await _semsterRepo.UpdateAsync(semster, cancellationToken);
                    }
                    break; // Add a break statement here

                default:
                    throw new InvalidOperationException("Invalid semester state.");
            }
        }

        
    }
}
*/