using Athena.Application.Features.StudentFeatures.Students.Dtos;
using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Students
{
    public record GetStudentBaseRequest() : IRequest<StudentBaseDto>;

    public class GetStudentBaseRequestHandler : IRequestHandler<GetStudentBaseRequest, StudentBaseDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IReadRepository<Student> _studentRepo;

        public GetStudentBaseRequestHandler(ICurrentUser currentUser, IUserService userService, IReadRepository<Student> studentRepo)
        {
            _currentUser = currentUser;
            _userService = userService;
            _studentRepo = studentRepo;
        }

        public async Task<StudentBaseDto> Handle(GetStudentBaseRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetByIdAsync(_currentUser.GetUserId(), cancellationToken);
            var user = await _userService.GetAsync(student!.Id.ToString(), cancellationToken);

            return new()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber, 
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Address = student.Address,
                ImagePath = student.Image,
                Code = student.Code,
            };
        }
    }
}
