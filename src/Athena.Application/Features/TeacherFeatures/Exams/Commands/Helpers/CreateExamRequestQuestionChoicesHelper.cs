namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public class CreateExamRequestQuestionChoicesHelper
    {
        public int Index { get; set; }
        public string Name { get; set; } = default!;
        public FileUploadRequest? Image { get; set; }
        public bool IsRightChoice { get; set; }
    }

    public class CreateExamRequestQuestionChoicesHelperValidator : CustomValidator<CreateExamRequestQuestionChoicesHelper>
    {
        public CreateExamRequestQuestionChoicesHelperValidator(IStringLocalizer<CreateExamRequestQuestionChoicesHelperValidator> T)
        {
            RuleFor(e => e.Name)
                 .NotEmpty()
                 .NotNull()
                 .WithMessage((_, name) => T["Name Can't Be Null"]);

            RuleFor(e => e.Image)
                .InjectValidator();
        }

    }
}
