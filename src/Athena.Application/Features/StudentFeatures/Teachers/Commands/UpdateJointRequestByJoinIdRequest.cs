using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Teachers.Commands
{
    public record UpdateJointRequestByJoinIdRequest(Guid Id, Guid GroupId) : IRequest<Guid>;

    public class UpdateJointRequestByJoinIdRequestValidator : CustomValidator<UpdateJointRequestByJoinIdRequest>
    {
        public UpdateJointRequestByJoinIdRequestValidator(IReadRepository<Group> groupRepo, IStringLocalizer<UpdateJointRequestByJoinIdRequestValidator> T)
        {
            RuleFor(e => e.GroupId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (groupId, ct) => await groupRepo.GetByIdAsync(groupId, ct) is not null)
                .WithMessage((_, groupId) => T["Group {0} Not Found!", groupId]);
        }
    }

    public class UpdateJointRequestByJoinIdRequestHandler : IRequestHandler<UpdateJointRequestByJoinIdRequest, Guid>
    {
        private readonly IRepository<StudentTeacherRequest> _studentTeacherRequestRepo;
        private readonly IStringLocalizer<UpdateJointRequestByJoinIdRequestHandler> _t;

        public UpdateJointRequestByJoinIdRequestHandler(
            IRepository<StudentTeacherRequest> studentTeacherRequestRepo, 
            IStringLocalizer<UpdateJointRequestByJoinIdRequestHandler> t)
        {
            _studentTeacherRequestRepo = studentTeacherRequestRepo;
            _t = t;
        }

        public async Task<Guid> Handle(UpdateJointRequestByJoinIdRequest request, CancellationToken cancellationToken)
        {
            var studentTeacherRequest = await _studentTeacherRequestRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = studentTeacherRequest ?? throw new NotFoundException(_t["StudentTeacherRequest {0} Not Found!", request.Id]);

            if (studentTeacherRequest.State != StudentTeacherRequestStatus.Pending)
                throw new ConflictException(_t["You can't update this request any more"]);

            studentTeacherRequest.Update(request.GroupId, null, null);
            await _studentTeacherRequestRepo.UpdateAsync(studentTeacherRequest, cancellationToken);
            return request.Id;
        }
    }
}
