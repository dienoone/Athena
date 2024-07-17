namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    // TODO: Need To Validate Index and Name:
    public class UpdateSectionRequestQuestionChoiceHelper
    {
        public Guid Id { get; set; }
        public int? Index { get; set; }
        public string? Name { get; set; }
        public FileUploadRequest? Image { get; set; }
        public bool? IsRightChoice { get; set; }
    }

    public class UpdateSectionRequestQuestionChoiceHelperValidator : CustomValidator<UpdateSectionRequestQuestionChoiceHelper>
    {
        public UpdateSectionRequestQuestionChoiceHelperValidator(IReadRepository<QuestionChoice> questionChoiceRepo, IStringLocalizer<UpdateSectionRequestQuestionChoiceHelperValidator> T)
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, ct) => await questionChoiceRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => T["QuestionChoise {0} Not Found!", id]);

            RuleFor(e => e.Image)
                .InjectValidator();
        }

    }
}
