namespace Athena.Application.Features.StudentFeatures.Students
{
    public class CreateStudentRequestValidator : CustomValidator<CreateStudentRequest>
    {
        public CreateStudentRequestValidator(IRepository<LevelClassification> repository, IStringLocalizer<CreateStudentRequestValidator> T)
        {
            RuleFor(e => e.CreateUser)
                 .InjectValidator();

            RuleFor(p => p.Image)
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
                .Matches("^0[0-9]{10}$")
                .WithMessage((_, parentPhone) => T["ParentPhone {0} must start with 0 and have a length of 11.", parentPhone]);

            RuleFor(e => e.HomePhone)
                .NotNull()
                .NotEmpty()
                .Matches("^[0-9]{7}$")
                .WithMessage((_, homePhone) => T["HomePhone {0} must be 7 digits.", homePhone]);

            RuleFor(e => e.LevelClassificationId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (levelClassificationId, ct) => await repository.GetByIdAsync(levelClassificationId, ct) is not null)
                .WithMessage((_, levelClassificationId) => T["LevelClassigication {0} Not Found.", levelClassificationId]);
        }
    }
}
