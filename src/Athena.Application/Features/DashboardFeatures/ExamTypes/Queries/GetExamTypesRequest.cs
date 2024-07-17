using Athena.Application.Features.DashboardFeatures.ExamTypes.Dtos;

namespace Athena.Application.Features.DashboardFeatures.ExamTypes.Queries
{
    public record GetExamTypesRequest() : IRequest<List<ExamTypeDto>>;

    public class GetExamTypesRequestHandler : IRequestHandler<GetExamTypesRequest, List<ExamTypeDto>>
    {
        private readonly IRepository<ExamType> _repository;

        public GetExamTypesRequestHandler(IRepository<ExamType> repository) => _repository = repository;

        public async Task<List<ExamTypeDto>> Handle(GetExamTypesRequest request, CancellationToken cancellationToken)
        {
            var examTypes = await _repository.ListAsync(cancellationToken);
            return examTypes.Adapt<List<ExamTypeDto>>();
        }
    }
}
