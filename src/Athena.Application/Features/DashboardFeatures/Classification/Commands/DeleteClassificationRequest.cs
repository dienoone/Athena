namespace Athena.Application.Features.DashboardFeatures.Classification.Commands
{
    public record DeleteClassificationRequest(Guid Id) : IRequest<Guid>;

    public class DeleteClassificationRequestHandler : IRequestHandler<DeleteClassificationRequest, Guid>
    {
        private readonly IRepository<EducationClassification> _repository;
        private readonly IStringLocalizer<DeleteClassificationRequestHandler> _t;

        public DeleteClassificationRequestHandler(IRepository<EducationClassification> repository, IStringLocalizer<DeleteClassificationRequestHandler> t) =>
            (_repository, _t) = (repository, t);

        public async Task<Guid> Handle(DeleteClassificationRequest request, CancellationToken cancellationToken)
        {
            var classification = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _ = classification ?? throw new NotFoundException(_t["Classification {0} Not Found!", request.Id]);

            await _repository.DeleteAsync(classification, cancellationToken);
            return request.Id;
        }
    }
}
