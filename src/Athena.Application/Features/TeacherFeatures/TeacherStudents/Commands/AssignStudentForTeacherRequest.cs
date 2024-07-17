using Athena.Application.Features.TeacherFeatures.Groups.Spec;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Commands
{
    // Check that the group level == student level
    public record AssignStudentForTeacherRequest(Guid StudnetId, Guid GroupId, int IntroFee) : IRequest<Guid>;

    public class AssignStudentForTeacherRequestValidator : CustomValidator<AssignStudentForTeacherRequest>
    {
        public AssignStudentForTeacherRequestValidator(ICurrentUser currentUser, IReadRepository<Student> studentRepo, IReadRepository<Group> groupRepo,
           IReadRepository<TeacherCourseLevelYearStudent> teacherCourseLevelStudentRepo, IStringLocalizer<AssignStudentForTeacherRequestValidator> T)
        {
            RuleFor(e => e.StudnetId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (studnetId, ct) => await studentRepo.GetByIdAsync(studnetId, ct) is not null)
                .WithMessage((_, studentId) => T["Studnet {0} Not Found.", studentId]);

            RuleFor(e => e.StudnetId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (studnetId, ct) => await teacherCourseLevelStudentRepo.GetBySpecAsync(new TeacherCourseLevelYearStudentByStudnetIdAndBusinessIdSpec(studnetId, currentUser.GetBusinessId()), ct) is null)
                .WithMessage((_, studentId) => T["Studnet {0} Already Assign To This Teacher.", studentId]);

            RuleFor(e => e.GroupId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (groupId, ct) => await groupRepo.GetByIdAsync(groupId, ct) is not null)
                .WithMessage((_, groupId) => T["Group {0} Not Found.", groupId]);
        }
    }

    public class AssignStudentForTeacherRequestHandler : IRequestHandler<AssignStudentForTeacherRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Group> _groupRepo;
        private readonly IRepository<GroupStudent> _groupStudentRepo;
        private readonly IRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;

        public AssignStudentForTeacherRequestHandler(ICurrentUser currentUser, IRepository<Group> groupRepo,
            IRepository<GroupStudent> groupStudentRepo, IRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo) =>
        (_currentUser, _groupRepo, _groupStudentRepo, _teacherCourseLevelYearStudentRepo) = (currentUser, groupRepo, groupStudentRepo, teacherCourseLevelYearStudentRepo);

        public async Task<Guid> Handle(AssignStudentForTeacherRequest request, CancellationToken cancellationToken)
        {
            // To Get Teacher Course Level Id
            var group = await _groupRepo.GetBySpecAsync(new TeacherCourseLevelByGroupIdGroupSpec(request.GroupId), cancellationToken);

            TeacherCourseLevelYearStudent teacherCourseLevelYearStudent = new(group!.TeacherCourseLevelYear.Id, request.StudnetId, request.IntroFee, _currentUser.GetBusinessId());
            await _teacherCourseLevelYearStudentRepo.AddAsync(teacherCourseLevelYearStudent, cancellationToken);

            GroupStudent groupStudent = new(request.GroupId, teacherCourseLevelYearStudent.Id, _currentUser.GetBusinessId());
            await _groupStudentRepo.AddAsync(groupStudent, cancellationToken);

            return teacherCourseLevelYearStudent.Id;
        }
    }
}
