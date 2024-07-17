using Athena.Application.Features.DashboardFeatures.ExamTypes.Dtos;

namespace Athena.Application.Features.DashboardFeatures.ExamTypes.Queries
{
    public record GetExamTypeByIdRequest(Guid Id) : IRequest<ExamTypeDto>;

    public class GetExamTypeByIdRequestHandler : IRequestHandler<GetExamTypeByIdRequest, ExamTypeDto>
    {
        private readonly IRepository<ExamType> _repository;
        private readonly IStringLocalizer _t;

        public GetExamTypeByIdRequestHandler(IRepository<ExamType> repository, IStringLocalizer<GetExamTypeByIdRequestHandler> localizer) =>
            (_repository, _t) = (repository, localizer);

        public async Task<ExamTypeDto> Handle(GetExamTypeByIdRequest request, CancellationToken cancellationToken)
        {

            var examType = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _ = examType ?? throw new NotFoundException(_t["ExamType {0} Not Found.", request.Id]);
            return examType.Adapt<ExamTypeDto>();
        }
    }
}
