using Athena.Application.Features.TeacherFeatures.Profile.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Profile.Commands
{
    // ToDo: Marwan Add Address
    public class UpdateTeacherProfileRequest : IRequest<Guid>
    {
        public FileUploadRequest? ProfileImage { get; set; }
        public bool IsProfileImageDeleted { get; set; }

        public FileUploadRequest? CoverImage { get; set; }
        public bool IsCoverImageDeleted { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Summary { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Nationality { get; set; }
        public string? Degree { get; set; }
        public string? Address { get; set; }
        public string? School { get; set; }
        public string? TeachingMethod { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set;}
        public string? WebSite { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Youtube { get; set; }

    }
    public class UpdateTeacherProfileRequestValidator : CustomValidator<UpdateTeacherProfileRequest>
    {
        public UpdateTeacherProfileRequestValidator()
        {
            RuleFor(e => e.ProfileImage)
                .InjectValidator();

            RuleFor(e => e.CoverImage)
                .InjectValidator();

            RuleFor(p => p.FirstName).Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(p => p.LastName).Cascade(CascadeMode.Stop)
               .NotEmpty();
        }
    }

    public class UpdateTeacherProfileRequestHandler : IRequestHandler<UpdateTeacherProfileRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IRepository<Teacher> _teacherRepo;
        private readonly IRepository<TeacherContact> _teacherContactRepo;
        private readonly IFileStorageService _file;
        private readonly IStringLocalizer<UpdateTeacherProfileRequestHandler> _t;

        public UpdateTeacherProfileRequestHandler(ICurrentUser currentUser, IUserService userService, IRepository<Teacher> teacherRepo, 
            IRepository<TeacherContact> teacherContactRepo, IFileStorageService file, IStringLocalizer<UpdateTeacherProfileRequestHandler> t)
        {
            _currentUser = currentUser;
            _userService = userService;
            _teacherRepo = teacherRepo;
            _teacherContactRepo = teacherContactRepo;
            _t = t;
            _file = file;

        }

        public async Task<Guid> Handle(UpdateTeacherProfileRequest request, CancellationToken cancellationToken)
        {
            var businessId = _currentUser.GetBusinessId();
            var userId = _currentUser.GetUserId();

            var teacher = await _teacherRepo.GetByIdAsync(userId, cancellationToken);
            _ = teacher ?? throw new InternalServerException(_t["Teacher Not Found!"]);

            string teacherName = request.FirstName + " " + request.LastName;
            teacher = await UpdateTeacher(request, teacher, teacherName, cancellationToken);

            await UpdateOrCreateTeacherContact(Contacts.Facebook, request.Facebook, userId, businessId, cancellationToken);
            await UpdateOrCreateTeacherContact(Contacts.Twitter, request.Twitter, userId, businessId, cancellationToken);
            await UpdateOrCreateTeacherContact(Contacts.WebSite, request.WebSite, userId, businessId, cancellationToken);
            await UpdateOrCreateTeacherContact(Contacts.Youtube, request.Youtube, userId, businessId, cancellationToken);

            await _userService.UpdateAsync(new() 
            { 
                Id = userId.ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                Email = request.Email,
            }, userId.ToString());

            await _teacherRepo.UpdateAsync(teacher, cancellationToken);

            return Guid.NewGuid();
        }

        private async Task UpdateOrCreateTeacherContact(string contact, string? data, Guid teacherId, Guid businessId, CancellationToken cancellationToken)
        {
            if (data == null) return;

            var contactDetail = await _teacherContactRepo.GetBySpecAsync(new TeacherContactByContactSpec(contact, teacherId), cancellationToken);
            if (contactDetail == null)
            {
                TeacherContact teacherContact = new(contact, data, teacherId, businessId);
                await _teacherContactRepo.AddAsync(teacherContact, cancellationToken);
            }
            else
            {
                contactDetail.Update(data);
                await _teacherContactRepo.UpdateAsync(contactDetail, cancellationToken);
            }
        }

        private async Task<Teacher> UpdateTeacher(UpdateTeacherProfileRequest request, Teacher? teacher, string teacherName, CancellationToken cancellationToken)
        {
            teacher = DeleteProfileImage(request, teacher);
            teacher = DeleteCoverImage(request, teacher);

            string? profileImagePath = request!.ProfileImage is not null
                ? await _file.UploadAsync<Teacher>(request.ProfileImage, FileType.Image, cancellationToken)
                : null;

            string? profileCoverPath = request!.CoverImage is not null
                ? await _file.UploadAsync<Teacher>(request.CoverImage, FileType.Image, cancellationToken)
                : null;


            var updatedTeacher = teacher.Update(teacherName, null, request.Address, profileImagePath, request.BirthDay, profileCoverPath, request.Summary, request.Nationality, request.Degree, request.School, request.TeachingMethod, null);
            
            return updatedTeacher;
        }

        private Teacher DeleteCoverImage(UpdateTeacherProfileRequest request, Teacher? teacher)
        {
            // Remove old cover image if flag is set
            if (request.IsCoverImageDeleted)
            {
                string? currentCoverImagePath = teacher!.CoverImagePath;
                if (!string.IsNullOrEmpty(currentCoverImagePath))
                {
                    string root = Directory.GetCurrentDirectory();
                    _file.Remove(Path.Combine(root, currentCoverImagePath));
                }

                teacher = teacher.ClearCoverImagePath();
            }

            return teacher!;
        }

        private Teacher DeleteProfileImage(UpdateTeacherProfileRequest request, Teacher? teacher)
        {
            // Remove old profile image if flag is set
            if (request.IsProfileImageDeleted)
            {
                string? currentProfileImagePath = teacher!.ImagePath;
                if (!string.IsNullOrEmpty(currentProfileImagePath))
                {
                    string root = Directory.GetCurrentDirectory();
                    _file.Remove(Path.Combine(root, currentProfileImagePath));
                }

                teacher = teacher.ClearImagePath();
            }

            return teacher!;
        }
    }
}
