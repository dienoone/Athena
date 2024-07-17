namespace Athena.Application.Features.StudentFeatures.Profile.Commands
{
    public record UpdateStudentEducationDetailsRequest(string School, Guid LevelClassificationId) : IRequest<Guid>;

    public class UpdateStudentEducationDetailsRequestHandler : IRequestHandler<UpdateStudentEducationDetailsRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Student> _studentRepo;

        public UpdateStudentEducationDetailsRequestHandler(ICurrentUser currentUser, IRepository<Student> studentRepo)
        {
            _currentUser = currentUser;
            _studentRepo = studentRepo;
        }

        public async Task<Guid> Handle(UpdateStudentEducationDetailsRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetByIdAsync(_currentUser.GetUserId(), cancellationToken);
            student!.Update(null, null, null, null, null, null, null, null, null, request.School, request.LevelClassificationId);

            await _studentRepo.UpdateAsync(student, cancellationToken);
            return student.Id;
        }
    }

}
