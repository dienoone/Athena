using Athena.Application.Features.TeacherFeatures.Years.Dtos;
using Athena.Application.Features.TeacherFeatures.Years.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Years.Queries
{
    public record GetYearsRequest() : IRequest<YearListRequestDto>;
    public class GetYearsRequestHandler : IRequestHandler<GetYearsRequest, YearListRequestDto>
    {
        private readonly ICurrentUser _currnetUser;
        private readonly IReadRepository<Year> _yearRepo;
        public GetYearsRequestHandler(ICurrentUser currnetUser, IReadRepository<Year> yearRepo) =>
            (_currnetUser, _yearRepo) = (currnetUser, yearRepo);

        public async Task<YearListRequestDto> Handle(GetYearsRequest request, CancellationToken cancellationToken)
        {
            var years = await _yearRepo.ListAsync(new YearListByBusinessIdSpec(_currnetUser.GetBusinessId()), cancellationToken);

            return new() 
            { 
                Open = years?.Where(e => e.YearState == YearStatus.Open).Adapt<List<YearListDto>>(),
                Preopen = years?.Where(e => e.YearState == YearStatus.Preopen).Adapt<List<YearListDto>>(),
                Finished = years?.Where(e => e.YearState == YearStatus.Finished).Adapt<List<YearListDto>>()
            };

        }
    }


}
