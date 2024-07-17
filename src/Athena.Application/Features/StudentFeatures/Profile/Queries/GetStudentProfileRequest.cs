using Athena.Application.Features.StudentFeatures.Profile.Dtos;
using Athena.Application.Features.StudentFeatures.Profile.Spec;
using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Profile.Queries
{
    public record GetStudentProfileRequest() : IRequest<StudentProfileDto>;

    public class GetStudentProfileRequestHandler : IRequestHandler<GetStudentProfileRequest, StudentProfileDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IReadRepository<Student> _studentRepo;

        public GetStudentProfileRequestHandler(ICurrentUser currentUser, IUserService userService, IReadRepository<Student> studentRepo)
        {
            _currentUser = currentUser;
            _userService = userService;
            _studentRepo = studentRepo;
        }

        public async Task<StudentProfileDto> Handle(GetStudentProfileRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetBySpecAsync(new StudentIncludeLevelClassificationByStudentIdSpec(_currentUser.GetUserId()), cancellationToken);
            var user = await _userService.GetAsync(student!.Id.ToString(), cancellationToken);

            return new() 
            { 
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Code = student.Code,
                School = student.School,
                Level = student.LevelClassification.Level.Name,
                Classification = student.LevelClassification.EducationClassification.Name,
                Image = student.Image,
                Gender = user.Gender,
                BirthDay = student.BirthDay,
                Address = student.Address,
                ParentName = student.ParentName,
                ParentJob = student.ParentJob,
                ParentPhone = student.ParentPhone,
                LevelClassificationId = student.LevelClassificationId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                HomePhone = student.HomePhone
            };
        }
    }
}
