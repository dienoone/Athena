namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Update
{
    public class UpdateExamRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public Guid ExamTypeId { get; set; }
        public string? Name { get; set; }
        public DateTime? PublishedDate { get; set; }
        public TimeSpan? PublishedTime { get; set; }
        public int? AllowedTime { get; set; }
        public bool? IsPrime { get; set; }
    }

    public class UpdateExamRequestValidator : CustomValidator<UpdateExamRequest>
    {
        // ToDo: Check Exam Name
        public UpdateExamRequestValidator(
                IReadRepository<ExamType> examTypeRepo,
                IStringLocalizer<UpdateExamRequestValidator> T)
        {
            RuleFor(e => e.ExamTypeId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (examTypeId, ct) => await examTypeRepo.GetByIdAsync(examTypeId, ct) is not null)
                .WithMessage((_, examTypeId) => T["ExamType {0} Not Found!", examTypeId]);
        }

    }

    public class UpdateExamRequestHandler : IRequestHandler<UpdateExamRequest, Guid>
    {
        private readonly IRepository<Exam> _examRepo;
        private readonly IStringLocalizer<UpdateExamRequestHandler> _t;
        public UpdateExamRequestHandler(
            IRepository<Exam> examRepo,
            IStringLocalizer<UpdateExamRequestHandler> t)
        {
            _examRepo = examRepo;
            _t = t;
        }

        public async Task<Guid> Handle(UpdateExamRequest request, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", request.Id]);

            exam!.Update(request.Name, null, null, null, request.AllowedTime, request.PublishedDate,
                request.PublishedTime, null, request.IsPrime, null, request.ExamTypeId);

            await _examRepo.UpdateAsync(exam, cancellationToken);
            return exam.Id;
        }
    }

}
