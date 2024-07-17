using Athena.Application.Features.TeacherFeatures.Groups.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Groups.Commands
{
    // Todo: Need To Check Group Scadual
    public class CreateGroupRequestValidator : CustomValidator<CreateGroupRequest>
    {
        public CreateGroupRequestValidator(IReadRepository<HeadQuarter> headRepo,
            IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo, IReadRepository<Group> groupRepo, IStringLocalizer<CreateGroupRequestValidator> T)
        {
            RuleFor(e => e.HeadQuarterId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (headQuarterId, ct) => await headRepo.GetByIdAsync(headQuarterId, ct) is not null)
                .WithMessage((_, headQuarterId) => T["HeadQuarter {0} Not Found!", headQuarterId]);

            RuleFor(e => e.TeacherCourseLevelYearId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (teahcerCourseLevelYearId, ct) => await teacherCourseLevelYearRepo.GetByIdAsync(teahcerCourseLevelYearId, ct) is not null)
                .WithMessage((_, teahcerCourseLevelYearId) => T["TeacherCourseLevelId {0} Not Found!", teahcerCourseLevelYearId]);

            RuleFor(e => e.Name)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (request, name, ct) =>
                    await groupRepo.GetBySpecAsync(new GroupByNameAndTeacherCourseLevelYearIdSpec(name, request.TeacherCourseLevelYearId), ct) is null)
                .WithMessage((request, name) => T["Group {0} With TeahcerCourseLeverYearId {1} is Aleardy Exists.", name, request.TeacherCourseLevelYearId]);

            RuleFor(e => e.GroupScaduals)
                .Must((groupscadual) => CheckGroupScadual(groupscadual, T));

        }

        private static bool CheckGroupScadual(List<CreateGroupScadual> groupScaduals, IStringLocalizer<CreateGroupRequestValidator> T)
        {
            if (groupScaduals.Count < 0)
                throw new ConflictException(T["You Must Add Scadual for this group"]);
            foreach (CreateGroupScadual groupScadual in groupScaduals)
            {
                if (!Days.IsDayOfWeek(groupScadual.Day.ToLower()))
                    throw new ConflictException(T["Invaild Day, This Day {0} is not a day of week", groupScadual.Day]);

                if (groupScadual.StartTime > groupScadual.EndTime)
                    throw new ConflictException(T["Invaild Time, Start Time Must Be Greater Than End Time"]);
            }
            return true;
        }
    }
}
