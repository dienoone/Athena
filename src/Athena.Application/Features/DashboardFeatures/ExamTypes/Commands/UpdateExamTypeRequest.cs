using Athena.Application.Features.DashboardFeatures.ExamTypes.Spec;

namespace Athena.Application.Features.DashboardFeatures.ExamTypes.Commands
{
    public record UpdateExamTypeRequest(Guid Id, string Name, int Index) : IRequest<Guid>;

    public class UpdateExamTypeRequestValidator : CustomValidator<UpdateExamTypeRequest>
    {
        public UpdateExamTypeRequestValidator(IRepository<ExamType> repository, IStringLocalizer<UpdateExamTypeRequestValidator> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(75)
                .MustAsync(async (examType, name, ct) =>
                    await repository.GetBySpecAsync(new ExamTypeByNameSpec(name), ct)
                        is not ExamType existingExamType || existingExamType.Id == examType.Id)
                .WithMessage((_, name) => T["ExamType {0} already Exists {1}.", name, _.Id]);

        }
    }

    public class UpdateExamTypeRequestHandler : IRequestHandler<UpdateExamTypeRequest, Guid>
    {
        // Add Domain Events automatically by using IRepositoryWithEvents
        private readonly IRepository<ExamType> _repository;
        private readonly IStringLocalizer _t;

        public UpdateExamTypeRequestHandler(IRepository<ExamType> repository, IStringLocalizer<UpdateExamTypeRequestHandler> localizer) =>
           (_repository, _t) = (repository, localizer);

        public async Task<Guid> Handle(UpdateExamTypeRequest request, CancellationToken cancellationToken)
        {
            var examType = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _ = examType ?? throw new NotFoundException(_t["ExamType {0} Not Found.", request.Id]);

            examType.Update(request.Name, request.Index);

            await _repository.UpdateAsync(examType, cancellationToken);

            return request.Id;
        }

    }
}
