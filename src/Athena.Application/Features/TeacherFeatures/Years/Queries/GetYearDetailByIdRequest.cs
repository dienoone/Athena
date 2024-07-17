using Athena.Application.Features.TeacherFeatures.Years.Dtos;
using Athena.Application.Features.TeacherFeatures.Years.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Years.Queries
{
    public record GetYearDetailByIdRequest(Guid Id) : IRequest<YearDetailDto>;
    public class GetYearDetailByIdRequestHandler : IRequestHandler<GetYearDetailByIdRequest, YearDetailDto>
    {
        private readonly IReadRepository<Year> _yearRepo;
        private readonly IStringLocalizer<GetYearDetailByIdRequestHandler> _t;
        public GetYearDetailByIdRequestHandler(IReadRepository<Year> yearRepo, IStringLocalizer<GetYearDetailByIdRequestHandler> t) =>
            (_yearRepo, _t) = (yearRepo, t);

        public async Task<YearDetailDto> Handle(GetYearDetailByIdRequest request, CancellationToken cancellationToken)
        {
            var year = await _yearRepo.GetBySpecAsync(new YearDetailByIdSpec(request.Id), cancellationToken);
            _ = year ?? throw new NotFoundException(_t["Year {0} Not Found!"]);
            var dto = year.Adapt<YearDetailDto>();

            dto.StartDate = year.TeacherCourseLevelYears
                        .SelectMany(e => e.TeacherCourseLevelYearSemsters
                            .Where(e => e.Semster == Semster.FirstSemster)
                            .Select(e => e.StartDate))
                        .Min();

            dto.EndDate = year.TeacherCourseLevelYears
                        .SelectMany(e => e.TeacherCourseLevelYearSemsters
                            .Where(e => e.Semster == Semster.SecondSemster)
                            .Select(e => e.StartDate))
                        .Max();
            return dto;
        }
    }


}
