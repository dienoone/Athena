using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec.GroupStudentSpec;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Commands
{
    public record UpdateStudentGroupRequest(Guid TeacherCourseLevelYearStudentId, Guid GroupId) : IRequest<Guid>;

    public class UpdateStudentGroupRequestValidator : CustomValidator<UpdateStudentGroupRequest>
    {
        public UpdateStudentGroupRequestValidator(IReadRepository<Group> groupRepo, IStringLocalizer<UpdateStudentGroupRequestValidator> t)
        {
            RuleFor(e => e.GroupId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (groupId, ct) => await groupRepo.GetByIdAsync(groupId, ct) is not null)
                .WithMessage((_, groupId) => t["Group {0} Not Found.", groupId]);
        }
    }

    public class UpdateStudentGroupRequestHandler : IRequestHandler<UpdateStudentGroupRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<TeacherCourseLevelYearStudent> _studentRepo;
        private readonly IRepository<GroupStudent> _groupStudentRepo;
        private readonly IStringLocalizer<UpdateStudentGroupRequestHandler> _t;

        public UpdateStudentGroupRequestHandler(ICurrentUser currentUser, IReadRepository<TeacherCourseLevelYearStudent> studentRepo,
            IRepository<GroupStudent> groupStudentRepo, IStringLocalizer<UpdateStudentGroupRequestHandler> t)
        {
            _currentUser = currentUser;
            _studentRepo = studentRepo;
            _groupStudentRepo = groupStudentRepo;
            _t = t;
        }

        // repsponse with teacherCourseLevelStudentId
        public async Task<Guid> Handle(UpdateStudentGroupRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetByIdAsync(request.TeacherCourseLevelYearStudentId, cancellationToken);
            _ = student ?? throw new NotFoundException(_t["TeacherCourseLevelYearStudent {0} Not Found.", request.TeacherCourseLevelYearStudentId]);

            var groupStudent = await _groupStudentRepo.GetBySpecAsync(new GroupStudentByStudentIdAndBusinessIdSpec(request.TeacherCourseLevelYearStudentId, _currentUser.GetBusinessId()), cancellationToken);
            groupStudent!.Update(request.GroupId);
            await _groupStudentRepo.UpdateAsync(groupStudent, cancellationToken);

            return request.TeacherCourseLevelYearStudentId;
        }
    }
}
