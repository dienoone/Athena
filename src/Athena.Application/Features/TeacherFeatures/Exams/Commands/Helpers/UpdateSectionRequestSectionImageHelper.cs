namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public class UpdateSectionRequestSectionImageHelper
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public FileUploadRequest Image { get; set; } = default!;
    }

    public class UpdateSectionRequestSectionImageHelperValidator : CustomValidator<UpdateSectionRequestSectionImageHelper>
    {
        public UpdateSectionRequestSectionImageHelperValidator(IReadRepository<SectionImage> sectionImageRepo, IStringLocalizer<UpdateSectionRequestSectionImageHelperValidator> T)
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, ct) => await sectionImageRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => T["SectionImage {0} Not Found!", id]);

            RuleFor(e => e.Image)
                .InjectValidator();

            RuleFor(e => e.Index)
                .LessThan(10)
                .GreaterThanOrEqualTo(0)
                .WithMessage((_, index) => T["Index {0} Must Be Greater Than Or Equal To 0 And Less Than 10", index]);

        }
    }
}
