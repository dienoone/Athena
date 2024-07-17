using Athena.Application.Features.TeacherFeatures.Groups.Dtos;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Queries
{
    public record GetStudentByCodeRequest(string Code) : IRequest<StudentByCodeDto>;

    public class GetStudentByCodeRequestValidator : CustomValidator<GetStudentByCodeRequest>
    {
        public GetStudentByCodeRequestValidator(IStringLocalizer<GetStudentByCodeRequestValidator> T)
        {
            RuleFor(e => e.Code)
                .NotEmpty()
                .NotNull()
                .WithMessage(T["Code Can't be null"]);
        }
    }

    public class GetStudentByCodeRequestHandler : IRequestHandler<GetStudentByCodeRequest, StudentByCodeDto>
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;
        private readonly IReadRepository<Group> _groupRepo;
        private readonly IReadRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;
        private readonly IStringLocalizer<GetStudentByCodeRequestHandler> _t;

        public GetStudentByCodeRequestHandler(IUserService userService, ICurrentUser currentUser, IReadRepository<Student> studentRepo,
            IRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo, IReadRepository<Group> groupRepo,
            IReadRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo, IStringLocalizer<GetStudentByCodeRequestHandler> t)
        {
            _userService = userService;
            _currentUser = currentUser;
            _studentRepo = studentRepo;
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
            _groupRepo = groupRepo;
            _teacherCourseLevelYearStudentRepo = teacherCourseLevelYearStudentRepo;
            _t = t;
        }

        public async Task<StudentByCodeDto> Handle(GetStudentByCodeRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetBySpecAsync(new StudentByCodeSpec(request.Code), cancellationToken);
            _ = student ?? throw new NotFoundException(_t["Studnet {0} Code NotFound.", request.Code]);

            var isStudentAssigned = await _teacherCourseLevelYearStudentRepo.GetBySpecAsync(new TeacherCourseLevelYearStudentByStudentIdSpec(student.Id, _currentUser.GetBusinessId()), cancellationToken);
            if (isStudentAssigned != null) throw new ConflictException(_t["Student {0} already assigned.", request.Code]);

            var userDetail = await _userService.GetAsync(student.Id.ToString(), cancellationToken);
            var openGroups = await _groupRepo.ListAsync(new GroupsForAssignStudentForTeachersByLevelIdAndBusinessIdSpec(_currentUser.GetBusinessId(), student.LevelClassification.LevelId, YearStatus.Open), cancellationToken);
            var preOpenGroups = await _groupRepo.ListAsync(new GroupsForAssignStudentForTeachersByLevelIdAndBusinessIdSpec(_currentUser.GetBusinessId(), student.LevelClassification.LevelId, YearStatus.Preopen), cancellationToken);

            return new()
            {
                Id = student.Id,
                Code = request.Code,
                FirstName = userDetail.FirstName,
                MiddleName = userDetail.MiddleName,
                LastName = userDetail.LastName,
                UserName = userDetail.UserName,
                LevelName = student.LevelClassification.Level.Name,
                EducationClassificationName = student.LevelClassification.EducationClassification.Name,
                Email = userDetail.Email,
                Image = student.Image,
                Gender = userDetail.Gender,
                BirthDay = student.BirthDay,
                Address = student.Address,
                HomePhone = student.HomePhone,
                ParentName = student.ParentName,
                ParentJob = student.ParentJob,
                ParentPhone = student.ParentPhone,
                Phone = userDetail.PhoneNumber,
                Open = openGroups?.Adapt<List<GroupsRequiredDto>>(),
                PreOpen = preOpenGroups?.Adapt<List<GroupsRequiredDto>>()
            };
        }
    }
}
