using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public class CreateExamRequestQuestionHelper
    {
        public int Index { get; set; }
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? Answer { get; set; }
        public double Degree { get; set; }
        public bool IsPrime { get; set; }

        public List<CreateExamRequestImageHelper>? Images { get; set; }
        public List<CreateExamRequestQuestionChoicesHelper>? Choices { get; set; }
    }

    public class CreateExamRequestQuestionHelperValidator : CustomValidator<CreateExamRequestQuestionHelper>
    {
        public CreateExamRequestQuestionHelperValidator(IStringLocalizer<CreateExamRequestQuestionHelperValidator> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage((_, name) => T["Question Name Can't Be Null"]);

            RuleFor(e => e.Type)
                .NotEmpty()
                .NotNull()
                .Must((type) => type == QuestionTypes.Written || type == QuestionTypes.MCQ)
                .WithMessage((_, type) => T["Invalid Question Type {0}", type]);

            RuleFor(e => e.Degree)
                .GreaterThan(1)
                .WithMessage(T["Degree Must Be Greate Than 1"]);

            RuleForEach(e => e.Images)
                .InjectValidator();

            When(e => e.Type == QuestionTypes.MCQ, () =>
            {
                RuleForEach(e => e.Choices)
                    .InjectValidator();

            });
        }
    }
}
