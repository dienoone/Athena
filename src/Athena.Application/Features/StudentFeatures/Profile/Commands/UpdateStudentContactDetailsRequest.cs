using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Profile.Commands
{
    public record UpdateStudentContactDetailsRequest(string Email, string PhoneNumber, string HomePhone) : IRequest<Guid>;

    public class UpdateStudentContactDetailsRequestHandler : IRequestHandler<UpdateStudentContactDetailsRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IRepository<Student> _studentRepo;

        public UpdateStudentContactDetailsRequestHandler(ICurrentUser currentUser, IUserService userService, IRepository<Student> studentRepo)
        {
            _currentUser = currentUser;
            _userService = userService;
            _studentRepo = studentRepo;
        }

        public async Task<Guid> Handle(UpdateStudentContactDetailsRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetByIdAsync(_currentUser.GetUserId(), cancellationToken);
            var user = await _userService.GetAsync(student!.Id.ToString(), cancellationToken);

            student.Update(null, null, null, null, null, null, null, null, request.HomePhone, null, null);
            await _userService.UpdateAsync(new() { Id = student.Id.ToString(), FirstName = user.FirstName, Email = request.Email, DeleteCurrentImage = false, Image = null, LastName = user.LastName, PhoneNumber = request.PhoneNumber }, student.Id.ToString());

            return student.Id;
        }
    }
}
