using Athena.Application.Features.DashboardFeatures.ExamTypes.Spec;

namespace Athena.Application.Features.DashboardFeatures.ExamTypes.Commands
{
    public record CreateExamTypeRequest(string Name, int Index) : IRequest<Guid>;

    public class CreateExamTypeRequestValidator : CustomValidator<CreateExamTypeRequest>
    {
        public CreateExamTypeRequestValidator(IReadRepository<ExamType> repository, IStringLocalizer<CreateExamTypeRequestValidator> T)
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(75)
                .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new ExamTypeByNameSpec(name), ct) is null)
                .WithMessage((_, name) => T["ExamType {0} aleardy exists.", name]);
        }
    }

    public class CreateExamTypeRequestHandler : IRequestHandler<CreateExamTypeRequest, Guid>
    {
        private readonly IRepository<ExamType> _repository;

        public CreateExamTypeRequestHandler(IRepository<ExamType> repository) => _repository = repository;

        public async Task<Guid> Handle(CreateExamTypeRequest request, CancellationToken cancellationToken)
        {
            var examType = new ExamType(request.Name, request.Index);

            await _repository.AddAsync(examType, cancellationToken);

            return examType.Id;
        }
    }
}
