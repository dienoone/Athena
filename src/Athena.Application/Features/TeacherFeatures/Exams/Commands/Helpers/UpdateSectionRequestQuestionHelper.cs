using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    // Question request need to split, Add DeleteQuestion
    public class UpdateSectionRequestQuestionHelper
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? Answer { get; set; }
        public double Degree { get; set; }
        public bool IsPrime { get; set; }
        /*public bool IsDeleted { get; set; }*/

        public List<UpdateSectionRequestQuestionChoiceHelper>? Choices { get; set; }
        public List<UpdateSectionRequestQuestionImageHelper>? Images { get; set; }

        public List<CreateExamRequestImageHelper>? NewImages { get; set; }
        public List<CreateExamRequestQuestionChoicesHelper>? NewChoices { get; set; }

    }

    public class UpdateSectionRequestQuestionHelperValidator : CustomValidator<UpdateSectionRequestQuestionHelper>
    {
        public UpdateSectionRequestQuestionHelperValidator(IReadRepository<Question> questionRepo, IStringLocalizer<UpdateSectionRequestQuestionHelperValidator> T)
        {
            RuleFor(e => e.Id)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (id, ct) => await questionRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => T["Question {0} Not Found!", id]);

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
