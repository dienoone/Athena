namespace Athena.Application.Features.DashboardFeatures.Teachers.Commands
{
    public class CreateTeacherRequestValidator : CustomValidator<CreateTeacherRequest>
    {
        [Obsolete]
        public CreateTeacherRequestValidator(IReadRepository<Course> courseRepo, IReadRepository<Level> levelRepo, IStringLocalizer<CreateTeacherRequestValidator> T)
        {
            RuleFor(e => e.User)
                 .InjectValidator();

            RuleFor(p => p.Image)
               .InjectValidator();

            RuleFor(e => e.CourseId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (courseId, ct) => await courseRepo.GetByIdAsync(courseId, ct) is not null)
                .WithMessage((_, courseId) => T["Course {0} not found.", courseId]);

            RuleFor(e => e.LevelIds)
                .MustAsync(async (_, levelIds, ct) => await CheckLevels(levelIds, levelRepo, T, ct));
        }

        private static async Task<bool> CheckLevels(List<Guid> levelIds, IReadRepository<Level> levelRepo, IStringLocalizer<CreateTeacherRequestValidator> T, CancellationToken cancellationToken)
        {
            foreach (var id in levelIds)
            {
                var level = await levelRepo.GetByIdAsync(id, cancellationToken);
                _ = level ?? throw new NotFoundException(T["Level {0} Not Found!", id]);
            }
            return true;
        }
    }

}
