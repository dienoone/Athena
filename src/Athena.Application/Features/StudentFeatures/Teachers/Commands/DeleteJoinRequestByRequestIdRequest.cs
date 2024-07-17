namespace Athena.Application.Features.StudentFeatures.Teachers.Commands
{
    public record DeleteJoinRequestByRequestIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteJoinRequestByRequestIdRequestHandler : IRequestHandler<DeleteJoinRequestByRequestIdRequest, Guid>
    {
        private readonly IRepository<StudentTeacherRequest> _studentTeacherRequestRepo;
        private readonly IStringLocalizer<DeleteJoinRequestByRequestIdRequest> _t;

        public DeleteJoinRequestByRequestIdRequestHandler(
            IRepository<StudentTeacherRequest> studentTeacherRequestRepo, 
            IStringLocalizer<DeleteJoinRequestByRequestIdRequest> t)
        {
            _studentTeacherRequestRepo = studentTeacherRequestRepo;
            _t = t;
        }

        public async Task<Guid> Handle(DeleteJoinRequestByRequestIdRequest request, CancellationToken cancellationToken)
        {;
            var studentTeacherRequest = await _studentTeacherRequestRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = studentTeacherRequest ?? throw new NotFoundException(_t["StudentTeacherRequest {0} Not Found!", request.Id]);

            await _studentTeacherRequestRepo.DeleteAsync(studentTeacherRequest, cancellationToken);
            return studentTeacherRequest.Id;
        }
    }
}
