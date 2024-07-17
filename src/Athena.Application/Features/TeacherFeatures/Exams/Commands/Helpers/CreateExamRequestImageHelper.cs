namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public class CreateExamRequestImageHelper
    {
        public FileUploadRequest Image { get; set; } = default!;
        public int Index { get; set; }

    }

    public class CreateExamRequestImageHelperValidator : CustomValidator<CreateExamRequestImageHelper>
    {
        public CreateExamRequestImageHelperValidator(IStringLocalizer<CreateExamRequestImageHelperValidator> T)
        {
            RuleFor(e => e.Image)
                .InjectValidator();

            RuleFor(e => e.Index)
                .LessThan(10)
                .GreaterThanOrEqualTo(0)
                .WithMessage((_, index) => T["Index {0} Must Be Greater Than Or Equal To 0 And Less Than 10", index]);

        }
    }
}
