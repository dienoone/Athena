using Athena.Application.Features.TeacherFeatures.Groups.Dtos;
using Athena.Application.Features.TeacherFeatures.Groups.Spec;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec.GroupStudentSpec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Queries
{
    public record GetStudentCardByStudentIdRequest(Guid TeacherCourseLevelYearStudentId) : IRequest<StudentCardDto>;

    public class GetStudentCardByStudentIdRequestHandler : IRequestHandler<GetStudentCardByStudentIdRequest, StudentCardDto>
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IReadRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;
        private readonly IReadRepository<Group> _groupRepo;
        private readonly IReadRepository<GroupStudent> _groupStudentRepo;
        private readonly IStringLocalizer<GetStudentByCodeRequestHandler> _t;

        public GetStudentCardByStudentIdRequestHandler(IUserService userService, ICurrentUser currentUser, IReadRepository<Student> studentRepo,
            IReadRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo,
            IReadRepository<GroupStudent> groupStudentRepo, IStringLocalizer<GetStudentByCodeRequestHandler> t, IReadRepository<Group> groupRepo)
        {
            _userService = userService;
            _currentUser = currentUser;
            _studentRepo = studentRepo;
            _teacherCourseLevelYearStudentRepo = teacherCourseLevelYearStudentRepo;
            _groupStudentRepo = groupStudentRepo;
            _t = t;
            _groupRepo = groupRepo;
        }

        public async Task<StudentCardDto> Handle(GetStudentCardByStudentIdRequest request, CancellationToken cancellationToken)
        {
            var teacherCourseLevelYearStudent = await _teacherCourseLevelYearStudentRepo.GetByIdAsync(request.TeacherCourseLevelYearStudentId, cancellationToken);
            _ = teacherCourseLevelYearStudent ?? throw new NotFoundException(_t["TeacherCourseLevelYearStudnet {0} Not Found.", request.TeacherCourseLevelYearStudentId]);

            var student = await _studentRepo.GetBySpecAsync(new StudentByIdSpec(teacherCourseLevelYearStudent.StudentId), cancellationToken);

            StudentCardDto card = new();

            #region Student Card Info:

            var userDetail = await _userService.GetAsync(student!.Id.ToString(), cancellationToken);
            var group = await _groupStudentRepo.GetBySpecAsync(new GroupStudentByStudentIdAndBusinessIdIncludeYearAndGroupSpec(student.Id, _currentUser.GetBusinessId()), cancellationToken);
            StudentCardInfoDto info = MapCardInfo(teacherCourseLevelYearStudent.Id, student, userDetail, group!);

            #region groups:

            var groups = await _groupRepo.ListAsync(new GroupsForUpdateStudentInfoByTeacherCourseLevelYearIdAndBuseinssIdSpec(group!.Group.TeacherCourseLevelYearId, YearStatus.Open, _currentUser.GetBusinessId()), cancellationToken);
            info.Groups = groups.Adapt<List<GroupsRequiredDto>>();


            card.Info = info;

            #endregion

            #endregion


            return card;
        }

        private static StudentCardInfoDto MapCardInfo(Guid studentId, Student student, UserDetailsDto userDetail, GroupStudent group)
        {
            return new()
            {
                Id = studentId,
                FirstName = userDetail.FirstName,
                MiddleName = userDetail.MiddleName,
                LastName = userDetail.LastName,
                Email = userDetail.Email,
                Image = student.Image,
                Gender = userDetail.Gender,
                Phone = userDetail.PhoneNumber,
                UserName = userDetail.UserName,
                Code = student.Code,
                LevelName = student.LevelClassification.Level.Name,
                EducationClassificationName = student.LevelClassification.EducationClassification.Name,
                BirthDay = student.BirthDay,
                Address = student.Address,
                HomePhone = student.HomePhone,
                ParentName = student.ParentName,
                ParentJob = student.ParentJob,
                ParentPhone = student.ParentPhone,
                GroupId = group!.Group.Id,
                GroupName = group.Group.Name
            };
        }
    }
}
