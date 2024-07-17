namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public class CreateExamRequestSectionHelper
    {
        public int Index { get; set; }
        public string Name { get; set; } = default!;
        public string? Paragraph { get; set; }
        public bool IsPrime { get; set; }
        public int? Time { get; set; }

        public List<CreateExamRequestImageHelper>? Images { get; set; }
        public List<CreateExamRequestQuestionHelper> Questions { get; set; } = default!;
    }

    public class CreateExamRequestSectionHelperValidator : CustomValidator<CreateExamRequestSectionHelper>
    {
        public CreateExamRequestSectionHelperValidator(IStringLocalizer<CreateExamRequestSectionHelperValidator> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage((_, name) => T["Section Name Can't Be Null"]);

            RuleForEach(e => e.Images)
                .InjectValidator();

            RuleFor(e => e.Questions)
                .Must((_, questions) => CheckQuestions(questions))
                .WithMessage(T["Questions Must Be Greate Than 1"]);

            RuleForEach(e => e.Questions)
                .InjectValidator();
        }

        private static bool CheckQuestions(List<CreateExamRequestQuestionHelper> questions)
        {
            if(questions != null)
                if (questions.Count >= 1) return true;
            return false;
        }
    }
}
