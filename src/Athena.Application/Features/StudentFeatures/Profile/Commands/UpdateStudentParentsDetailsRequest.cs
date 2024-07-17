namespace Athena.Application.Features.StudentFeatures.Profile.Commands
{
    public record UpdateStudentParentsDetailsRequest(string ParentName, string ParentJob, string ParentPhone) : IRequest<Guid>;

    public class UpdateStudentParentsDetailsRequestHandler : IRequestHandler<UpdateStudentParentsDetailsRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Student> _studentRepo;

        public UpdateStudentParentsDetailsRequestHandler(ICurrentUser currentUser, IRepository<Student> studentRepo)
        {
            _currentUser = currentUser;
            _studentRepo = studentRepo;
        }

        public async Task<Guid> Handle(UpdateStudentParentsDetailsRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetByIdAsync(_currentUser.GetUserId(), cancellationToken);

            student!.Update(null, null, null, null, null, request.ParentName, request.ParentJob, request.ParentPhone, null, null, null);
            await _studentRepo.UpdateAsync(student, cancellationToken);
            return student.Id;
        }
    }

}
