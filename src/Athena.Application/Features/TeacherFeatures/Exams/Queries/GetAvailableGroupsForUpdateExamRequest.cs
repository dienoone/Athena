using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;

namespace Athena.Application.Features.TeacherFeatures.Exams.Queries
{
    public record GetAvailableGroupsForUpdateExamRequest(Guid Id) : IRequest<List<GroupsForCreateExamDto>>;

    public class GetAvailableGroupsForUpdateExamRequestHandler : IRequestHandler<GetAvailableGroupsForUpdateExamRequest, List<GroupsForCreateExamDto>>
    {
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<ExamGroup> _examGroupRepo;
        private readonly IReadRepository<Group> _groupRepo;
        private readonly IStringLocalizer<GetAvailableGroupsForUpdateExamRequestHandler> _t;

        public GetAvailableGroupsForUpdateExamRequestHandler(IReadRepository<Exam> examRepo, IReadRepository<Group> groupRepo,
            IReadRepository<ExamGroup> examGroupRepo, IStringLocalizer<GetAvailableGroupsForUpdateExamRequestHandler> t)
        {
            _examRepo = examRepo;
            _examGroupRepo = examGroupRepo;
            _groupRepo = groupRepo;
            _t = t;
        }

        public async Task<List<GroupsForCreateExamDto>> Handle(GetAvailableGroupsForUpdateExamRequest request, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetBySpecAsync(new ExamIncludeSemseterByExamIdSpec(request.Id), cancellationToken);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", request.Id]);

            var examGroup = await _examGroupRepo.ListAsync(new ExamGroupsIncludeGroupByExamIdSpec(exam.Id), cancellationToken);
            var unAssignedGroups = await _groupRepo.ListAsync(new AvailableGroupsByFilterAndTeacherCourserLevelYearIdSpec(exam.TeacherCourseLevelYearId,
                examGroup.Select(e => e.GroupId).ToList()), cancellationToken);


            return unAssignedGroups.Adapt<List<GroupsForCreateExamDto>>();
        }
    }
}
