using Athena.Application.Features.TeacherFeatures.Groups.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Groups.Commands
{
    public class UpdateGroupRequestValidator : CustomValidator<UpdateGroupRequest>
    {
        public UpdateGroupRequestValidator(ICurrentUser currentUser, IReadRepository<HeadQuarter> headRepo,
            IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo, IReadRepository<Group> groupRepo,
            IReadRepository<GroupScadual> groupScadualRepo, IStringLocalizer<UpdateGroupRequestValidator> T)
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
            .NotNull()
            .NotEmpty()
            .MustAsync(async (request, name, ct) =>
                    await groupRepo.GetBySpecAsync(new GroupByNameAndTeacherCourseLevelYearIdSpec(name, request.TeacherCourseLevelYearId), ct)
                        is not Group existingGroup || existingGroup.Id == request.Id)
                .WithMessage((_, name) => T["Group {0} already exist.", name]);

            RuleFor(e => e.GroupScaduals)
                .MustAsync(async (groupScaduals, ct) => await CheckGroupScaduals(groupScaduals, groupScadualRepo, T, ct));

            RuleFor(e => e.NewGroupScaduals)
                .MustAsync(async (request, newGroupscadual, ct) => await CheckNewGroupScadual(newGroupscadual, request.Id, groupScadualRepo, T, ct));

        }

        private static async Task<bool> CheckGroupScaduals(List<UpdateGroupScadual>? groupScaduals, IReadRepository<GroupScadual> groupScadualRepo,
            IStringLocalizer<UpdateGroupRequestValidator> T, CancellationToken cancellationToken)
        {
            if(groupScaduals != null)
            {
                foreach (UpdateGroupScadual groupScadual in groupScaduals)
                {
                    var scadual = await groupScadualRepo.GetByIdAsync(groupScadual.Id, cancellationToken);
                    _ = scadual ?? throw new NotFoundException(T["GroupScadual {0} Not Found!", groupScadual.Id]);

                    if (!Days.IsDayOfWeek(groupScadual.Day.ToLower()))
                        throw new ConflictException(T["Invaild Day, This Day {0} is not a day of week", groupScadual.Day]);

                    if (groupScadual.StartTime > groupScadual.EndTime)
                        throw new ConflictException(T["Invaild Time, Start Time Must Be Greater Than End Time"]);
                }
            }

            return true;
        }

        private static async Task<bool> CheckNewGroupScadual(List<CreateGroupScadual>? groupScaduals, Guid groupId,
            IReadRepository<GroupScadual> groupScadualRepo, IStringLocalizer<UpdateGroupRequestValidator> T, CancellationToken cancellationToken)
        {
            if(groupScaduals != null)
            {
                foreach (CreateGroupScadual groupScadual in groupScaduals)
                {
                    if (!Days.IsDayOfWeek(groupScadual.Day.ToLower()))
                        throw new ConflictException(T["Invaild Day, This Day {0} is not a day of week", groupScadual.Day]);

                    if (groupScadual.StartTime > groupScadual.EndTime)
                        throw new ConflictException(T["Invaild Time, Start Time Must Be Greater Than End Time"]);

                    var isGroupSadualExist = await groupScadualRepo.GetBySpecAsync(new GroupScadualByDayAndGroupIdSpec(groupScadual.Day, groupId), cancellationToken);
                    if (isGroupSadualExist != null) throw new ConflictException(T["Group Scadual With Day {0} Already Exist.", groupScadual.Day]);
                }
            }
            return true;
        }
    }
}
