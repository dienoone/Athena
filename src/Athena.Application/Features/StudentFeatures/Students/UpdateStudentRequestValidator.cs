namespace Athena.Application.Features.StudentFeatures.Students
{
    public class UpdateStudentRequestValidator : CustomValidator<UpdateStudentRequest>
    {
        public UpdateStudentRequestValidator(IStringLocalizer<UpdateStudentRequestValidator> T)
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(e => e.User)
                 .InjectValidator();

            RuleFor(e => e.ParentName)
                .NotNull()
                .NotEmpty()
                .WithMessage((_, parentName) => T["ParentName {0} cannot be empty.", parentName]);

            RuleFor(e => e.ParentJob)
                .NotNull()
                .NotEmpty()
                .WithMessage((_, parentJop) => T["ParentJop {0} cannot be empty.", parentJop]);

            RuleFor(e => e.ParentPhone)
                .NotNull()
                .NotEmpty()
                .WithMessage((_, parentPhone) => T["ParentPhone {0} cannot be empty.", parentPhone]);

            RuleFor(e => e.HomePhone)
                .NotNull()
                .NotEmpty()
                .WithMessage((_, homePhone) => T["HomePhone {0} cannot be empty.", homePhone]);

            RuleFor(e => e.LevelId)
                .NotEmpty()
                .NotNull()
                /*.MustAsync(async (levelId, ct) => await repository.GetByIdAsync(levelId, ct) is not null)
                .WithMessage((_, levelId) => T["Level {0} Not Found.", levelId])*/;
        }
    }
}
