using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Teachers.Commands
{
    public class JoinTeacherRequestValidator : CustomValidator<JoinTeacherRequest>
    {
        public JoinTeacherRequestValidator(
            ICurrentUser currentUser,
            IReadRepository<StudentTeacherCommunication> studentTeacherCommunication,
            IReadRepository<StudentTeacherRequest> studentTeacherRequest,
            IReadRepository<Teacher> teacherRepo, 
            IReadRepository<Group> groupRepo,
            IStringLocalizer<JoinTeacherRequestValidator> T)
        {
            RuleFor(e => e.TeacherId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (teacherId, ct) => await teacherRepo.GetByIdAsync(teacherId, ct) is not null)
                .WithMessage((_, teacherId) => T["Teacher {0} Not Found!", teacherId]);

            RuleFor(e => e.GroupId)
               .NotEmpty()
               .NotNull()
               .MustAsync(async (groupId, ct) => await groupRepo.GetByIdAsync(groupId, ct) is not null)
               .WithMessage((_, groupId) => T["Group {0} Not Found!", groupId]);

            RuleFor(e => e.TeacherId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (teacherId, ct) => 
                    await CheckRequest(teacherId, currentUser.GetUserId(), studentTeacherCommunication, studentTeacherRequest, T, ct));

            RuleFor(e => e.TeacherId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (request, teacherId, ct) =>
                    await CheckGroup(teacherId, request.GroupId, currentUser.GetUserId(), studentTeacherRequest, T, ct));
        }

        private static async Task<bool> CheckRequest(
            Guid teacherId,
            Guid studentId,
            IReadRepository<StudentTeacherCommunication> studentTeacherCommunication,
            IReadRepository<StudentTeacherRequest> studentTeacherRequest,
            IStringLocalizer<JoinTeacherRequestValidator> T,
            CancellationToken cancellationToken)
        {
            var requests = await studentTeacherRequest.ListAsync(new StudentTeacherRequestByTeacherIdAndStudentIdSpec(teacherId, studentId), cancellationToken);

            if(requests != null)
            {
                if(requests.Count >= 3)
                {
                    var communication = await studentTeacherCommunication.GetBySpecAsync(new StudentTeacherCommunicationByStudentIdAndTeacherIdSpec(studentId, teacherId), cancellationToken);
                    if (!communication!.CanSendAgain)
                    {
                        throw new ConflictException(T["You have reached the maximum number of retry requests."]);
                    }
                }
            }
            return true;
        }

        private static async Task<bool> CheckGroup(
            Guid teacherId,
            Guid groupId,
            Guid studentId,
            IReadRepository<StudentTeacherRequest> studentTeacherRequest,
            IStringLocalizer<JoinTeacherRequestValidator> T,
            CancellationToken cancellationToken)
        {
            var requests = await studentTeacherRequest.ListAsync(new StudentTeacherRequestByTeacherIdAndStudentIdAndGroupIdSpec(teacherId, studentId, groupId), cancellationToken);

            if (requests.Any(e => e.State == StudentTeacherRequestStatus.Pending))
            {
                throw new ConflictException(T["Can't send to this group because the request is pendding."]);
            }
            return true;
        }
    }
}
