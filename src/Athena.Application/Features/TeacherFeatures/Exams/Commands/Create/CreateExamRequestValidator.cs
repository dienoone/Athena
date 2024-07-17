using Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Create
{
    public class CreateExamRequestValidator : CustomValidator<CreateExamRequest>
    {
        public CreateExamRequestValidator(IReadRepository<ExamType> examTypeRepo, IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo,
            IReadRepository<Group> groupRepo, IStringLocalizer<CreateExamRequestValidator> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage((_, name) => T["Exam Name Can't Be Null"]);

            RuleFor(e => e.ExamTypeId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (examTypeId, ct) => await examTypeRepo.GetByIdAsync(examTypeId, ct) is not null)
                .WithMessage((_, examTypeId) => T["ExamType {0} Not Found.", examTypeId]);

            RuleFor(e => e.GroupIds)
                .MustAsync(async (_, groupIds, ct) => await CheckGroupIds(groupIds, groupRepo, T, ct));

            RuleFor(e => e.Sections)
                .Must((_, sections) => CheckSections(sections))
                .WithMessage(T["Sections Must Be Greater Than 1"]);

            RuleForEach(e => e.Sections)
                .InjectValidator();
        }

        private static bool CheckSections(List<CreateExamRequestSectionHelper> sections)
        {
            if (sections != null)
                if (sections.Count >= 1) return true;
            return false;
        }

        private static async Task<bool> CheckGroupIds(List<Guid> Ids, IReadRepository<Group> groupRepo, IStringLocalizer<CreateExamRequestValidator> T, CancellationToken cancellationToken)
        {
            foreach (Guid id in Ids)
            {
                var group = await groupRepo.GetByIdAsync(id, cancellationToken);
                _ = group ?? throw new NotFoundException(T["Group {0} Not Found.", id]);
            }
            return true;
        }
    }
}
