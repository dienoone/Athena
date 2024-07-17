using Athena.Application.Features.StudentFeatures.Teachers.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Teachers.Queries
{
    // ToDo: continue this class
    public record GetTeacherDetailsRequest(Guid Id) : IRequest<TeacherDetailsDto>;

    public class GetTeacherDetailsRequestHandler : IRequestHandler<GetTeacherDetailsRequest, TeacherDetailsDto>
    {
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<GroupStudent> _groupStudentRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<GetTeacherDetailsRequestHandler> _t;

        public GetTeacherDetailsRequestHandler(IReadRepository<Teacher> teacherRepo, IReadRepository<GroupStudent> groupStudentRepo,
            ICurrentUser currentUser, IUserService userService, IStringLocalizer<GetTeacherDetailsRequestHandler> t)
        {
            _teacherRepo = teacherRepo;
            _groupStudentRepo = groupStudentRepo;
            _currentUser = currentUser;
            _userService = userService;
            _t = t;
        }

        public async Task<TeacherDetailsDto> Handle(GetTeacherDetailsRequest request, CancellationToken cancellationToken)
        {
            var studentId = _currentUser.GetUserId();
            var teacher = await _userService.GetAsync(request.Id.ToString(), cancellationToken);
            _ = teacher ?? throw new NotFoundException(_t["Teacher {0} Not Found!", request.Id]);


            var queryTeacher = await _teacherRepo.GetBySpecAsync(new TeacherIncludeContactsAndCourseByTeacherIdSpec(request.Id), cancellationToken);
            var groupStudent = await _groupStudentRepo.GetBySpecAsync(new GroupStudentIncludeScadualsByStudentIdAndTeacherIdSpec(teacher.Id, studentId), cancellationToken);

            return new() 
            { 
                Id = request.Id,
                Details = MapProfileDto(queryTeacher!),
                ContactDetails = MapContactDetails(teacher, queryTeacher!),
                TimeTable = MapTimeTableDto(groupStudent!.Group)
            };
        }

        private static TeacherProfileDetailsDto MapProfileDto(Teacher teacher)
        {
            return new TeacherProfileDetailsDto() 
            { 
                CoverImage = teacher.CoverImagePath,
                Image = teacher.ImagePath,
                Name = teacher.Name,
                CourseId = teacher.CourseId,
                Course = teacher.Course.Name,
                BirthDay = teacher.BirthDay,
                Address = teacher.Address,
                Nationality = teacher.Nationality,
                School = teacher.School,
                Education = teacher.Degree,
                TeachingTypes = teacher.TeachingMethod
            };
        }

        private static TeacherContactDetailsDto MapContactDetails(UserDetailsDto userDetailDto, Teacher teacher)
        {
            return new() 
            {
                Phone = userDetailDto.PhoneNumber,
                Email = userDetailDto.Email,
                Facebook = teacher.TeacherContacts != null ? teacher.TeacherContacts.SingleOrDefault(e => e.Contact == Contacts.Facebook)?.Contact : null,
                Twitter = teacher.TeacherContacts != null ? teacher.TeacherContacts.SingleOrDefault(e => e.Contact == Contacts.Twitter)?.Contact : null,
                Website = teacher.TeacherContacts != null ? teacher.TeacherContacts.SingleOrDefault(e => e.Contact == Contacts.WebSite)?.Contact : null,
                Youtube = teacher.TeacherContacts != null ? teacher.TeacherContacts.SingleOrDefault(e => e.Contact == Contacts.Youtube)?.Contact : null
            };
        }

        private static TeacherTimeTableDto MapTimeTableDto(Group group)
        {
            return new() 
            { 
                GroupId = group.Id,
                GroupName = group.Name,
                DaysOfAttendances = group.GroupScaduals.Adapt<List<DaysOfAttendanceDto>>()
            };
        }
    }

}
