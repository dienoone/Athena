using Athena.Application.Features.DashboardFeatures.Classification.Dtos;

namespace Athena.Application.Features.DashboardFeatures.Classification.Queries
{
    public record GetClassificationsRequest() : IRequest<List<ClassificationDto>>;

    public class GetClassificationsRequestHandler : IRequestHandler<GetClassificationsRequest, List<ClassificationDto>>
    {
        private readonly IRepository<EducationClassification> _repository;
        public GetClassificationsRequestHandler(IRepository<EducationClassification> repository)
        {
            _repository = repository;
        }

        public async Task<List<ClassificationDto>> Handle(GetClassificationsRequest request, CancellationToken cancellationToken)
        {
            var classifications = await _repository.ListAsync(cancellationToken);
            return classifications.Adapt<List<ClassificationDto>>();
        }
    }
}
