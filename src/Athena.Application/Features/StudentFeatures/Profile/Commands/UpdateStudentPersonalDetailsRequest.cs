using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Profile.Commands
{
    public record UpdateStudentPersonalDetailsRequest(string FirstName, string LastName, string MiddleName, string Gender, string Address, DateTime BirthDay) : IRequest<Guid>;

    public class UpdateStudentPersonalDetailsRequestHandler : IRequestHandler<UpdateStudentPersonalDetailsRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IRepository<Student> _studentRepo;

        public UpdateStudentPersonalDetailsRequestHandler(ICurrentUser currentUser, IUserService userService, IRepository<Student> studentRepo)
        {
            _currentUser = currentUser;
            _userService = userService;
            _studentRepo = studentRepo;
        }

        public async Task<Guid> Handle(UpdateStudentPersonalDetailsRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetByIdAsync(_currentUser.GetUserId(), cancellationToken);
            var user = await _userService.GetAsync(student!.Id.ToString(), cancellationToken);

            student.Update(null, null, request.Address, null, request.BirthDay, null, null, null, null, null, null);
            await _studentRepo.UpdateAsync(student, cancellationToken);
            await _userService.UpdateAsync(new() { Id = student.Id.ToString(), FirstName = request.FirstName, Email = user.Email, DeleteCurrentImage = false, Image = null, LastName = request.LastName, PhoneNumber = user.PhoneNumber }, student.Id.ToString());

            return student.Id;
        }
    }

}
