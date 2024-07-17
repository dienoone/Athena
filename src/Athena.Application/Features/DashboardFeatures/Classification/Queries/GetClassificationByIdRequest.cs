using Athena.Application.Features.DashboardFeatures.Classification.Dtos;

namespace Athena.Application.Features.DashboardFeatures.Classification.Queries
{

    public record GetClassificationByIdRequest(Guid Id) : IRequest<ClassificationDto>;

    public class GetClassificationByIdRequestHandler : IRequestHandler<GetClassificationByIdRequest, ClassificationDto>
    {
        private readonly IRepository<EducationClassification> _repository;
        private readonly IStringLocalizer _t;

        public GetClassificationByIdRequestHandler(IRepository<EducationClassification> repository, IStringLocalizer<GetClassificationByIdRequestHandler> t) =>
            (_repository, _t) = (repository, t);

        public async Task<ClassificationDto> Handle(GetClassificationByIdRequest request, CancellationToken cancellationToken)
        {
            var classification = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _ = classification ?? throw new NotFoundException(_t["Classification {0} Not Found.", request.Id]);
            return classification.Adapt<ClassificationDto>();
        }
    }
}
