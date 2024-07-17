using Athena.Application.Features.TeacherFeatures.Profile.Dtos;
using Athena.Application.Features.TeacherFeatures.Profile.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Profile.Queries
{
    public record GetTeacherProfileReqeust() : IRequest<ProfileTeacherDto>;

    public class GetTeacherProfileReqeustHandler : IRequestHandler<GetTeacherProfileReqeust, ProfileTeacherDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IReadRepository<Teacher> _teacherRepo;

        public GetTeacherProfileReqeustHandler(ICurrentUser currentUser, IUserService userService, IReadRepository<Teacher> teacherRepo)
        {
            _currentUser = currentUser;
            _userService = userService;
            _teacherRepo = teacherRepo;
        }

        public async Task<ProfileTeacherDto> Handle(GetTeacherProfileReqeust request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            var queryTeacher = await _teacherRepo.GetBySpecAsync(new TeacherIncludeTeacherContanctsByTeacherIdSpec(userId), cancellationToken);
            var userData = await _userService.GetAsync(userId.ToString(), cancellationToken);

            return new ProfileTeacherDto() 
            { 
                Image = queryTeacher!.ImagePath,
                CoverImage = queryTeacher.CoverImagePath,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Course = queryTeacher.Course.Name,
                BirthDay = queryTeacher.BirthDay,
                HeadQuarters = queryTeacher.HeadQuarters.Select(e => e.Name).ToList(),
                Nationality = queryTeacher.Nationality,
                School = queryTeacher.School,
                Degree = queryTeacher.Degree,
                TeachingMethod = queryTeacher.TeachingMethod,
                
                Phone = userData.PhoneNumber,
                Email = userData.Email,
                WebSite = queryTeacher.TeacherContacts.Where(e => e.Contact == Contacts.WebSite).FirstOrDefault()?.Data,
                Facebook = queryTeacher.TeacherContacts.Where(e => e.Contact == Contacts.Facebook).FirstOrDefault()?.Data,
                Twitter = queryTeacher.TeacherContacts.Where(e => e.Contact == Contacts.Twitter).FirstOrDefault()?.Data,
                Youtube = queryTeacher.TeacherContacts.Where(e => e.Contact == Contacts.Youtube).FirstOrDefault()?.Data,
                
                Summary = queryTeacher.Summary
            };
        }
    }
}
