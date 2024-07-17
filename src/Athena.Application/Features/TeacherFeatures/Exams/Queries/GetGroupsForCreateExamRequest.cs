using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;

namespace Athena.Application.Features.TeacherFeatures.Exams.Queries
{
    public record GetGroupsForCreateExamRequest(Guid TeacherCourseLevelYearId) : IRequest<List<GroupsForCreateExamDto>>;

    public class GetGroupsForCreateExamRequestValidator : CustomValidator<GetGroupsForCreateExamRequest>
    {
        public GetGroupsForCreateExamRequestValidator(IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo, IStringLocalizer<GetGroupsForCreateExamRequestValidator> T)
        {
            RuleFor(e => e.TeacherCourseLevelYearId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (teacherCourseLevelYearId, ct) => await teacherCourseLevelYearRepo.GetByIdAsync(teacherCourseLevelYearId, ct) is not null)
                .WithMessage((_, teacherCourseLevelYearId) => T["TeacherCourseLevelYear {0} Not Found.", teacherCourseLevelYearId]);
        }
    }

    public class GetGroupsForCreateExamRequestHandler : IRequestHandler<GetGroupsForCreateExamRequest, List<GroupsForCreateExamDto>>
    {
        private readonly IReadRepository<Group> _groupRepo;

        public GetGroupsForCreateExamRequestHandler(IReadRepository<Group> groupRepo) => _groupRepo = groupRepo;

        public async Task<List<GroupsForCreateExamDto>> Handle(GetGroupsForCreateExamRequest request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepo.ListAsync(new GroupsForCreateExamByTeacherCourseLevelYearIdSpec(request.TeacherCourseLevelYearId), cancellationToken);
            return groups.Adapt<List<GroupsForCreateExamDto>>();
        }
    }

}
