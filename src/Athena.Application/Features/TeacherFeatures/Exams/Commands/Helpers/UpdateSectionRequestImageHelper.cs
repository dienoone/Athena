namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public class UpdateSectionRequestQuestionImageHelper
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public FileUploadRequest Image { get; set; } = default!;
        /*public bool IsDeleted { get; set; }*/
    }

    public class UpdateSectionRequestQuestionImageHelperValidator : CustomValidator<UpdateSectionRequestQuestionImageHelper>
    {
        public UpdateSectionRequestQuestionImageHelperValidator(IReadRepository<QuestionImage> questionImageRepo, IStringLocalizer<UpdateSectionRequestQuestionImageHelperValidator> T)
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, ct) => await questionImageRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => T["QuestionImage {0} Not Found!", id]);

            RuleFor(e => e.Image)
                .InjectValidator();

            RuleFor(e => e.Index)
                .LessThan(10)
                .GreaterThanOrEqualTo(0)
                .WithMessage((_, index) => T["Index {0} Must Be Greater Than Or Equal To 0 And Less Than 10", index]);

        }
    }
}
