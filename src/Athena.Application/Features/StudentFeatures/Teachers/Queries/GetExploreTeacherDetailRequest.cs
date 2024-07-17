using Athena.Application.Features.StudentFeatures.Teachers.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Teachers.Queries
{
    // teacherId
    public record GetExploreTeacherDetailRequest(Guid Id) : IRequest<ExploreTeacherDetailDto>;

    public class GetExploreTeacherDetailRequestHandler : IRequestHandler<GetExploreTeacherDetailRequest, ExploreTeacherDetailDto>
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<Student> _studentRepo;
        private readonly IReadRepository<Year> _yearRepo;
        private readonly IReadRepository<StudentTeacherCommunication> _studentTeacherCommunicationRepo;
        private readonly IReadRepository<StudentTeacherRequest> _studentTeacherRequestRepo;
        private readonly IStringLocalizer<GetExploreTeacherDetailRequestHandler> _t;

        public GetExploreTeacherDetailRequestHandler(
            IUserService userService,
            ICurrentUser currentUser,
            IReadRepository<Teacher> teacherRepo,
            IReadRepository<Student> studentRepo,
            IReadRepository<Year> yearRepo,
            IReadRepository<StudentTeacherCommunication> studentTeacherCommunicationRepo,
            IReadRepository<StudentTeacherRequest> studentTeacherRequestRepo,
            IStringLocalizer<GetExploreTeacherDetailRequestHandler> t)
        {
            _userService = userService;
            _currentUser = currentUser;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _yearRepo = yearRepo;
            _studentTeacherCommunicationRepo = studentTeacherCommunicationRepo;
            _studentTeacherRequestRepo = studentTeacherRequestRepo;
            _t = t;
        }

        public async Task<ExploreTeacherDetailDto> Handle(GetExploreTeacherDetailRequest request, CancellationToken cancellationToken)
        {
            var teacher = await GetTeacherAsync(request.Id, cancellationToken);
            var student = await GetStudentAsync(cancellationToken);
            var teacherUser = await _userService.GetAsync(teacher.Id.ToString(), cancellationToken);
            var exploreTeachers = await _teacherRepo.ListAsync(new SmallExploreTeachersByLeveIdAndStudentIdAndCourseIdExcludeTeacherIdSpec(student!.Id, student.LevelClassification.LevelId, teacher.CourseId, teacher.Id), cancellationToken);
            List<ExploreTeacherDto> exploreTeacherDtos = await GetExploreTeacherDtos(exploreTeachers, cancellationToken);
            var openYear = await GetYearAsync(YearStatus.Open, teacher.BusinessId, cancellationToken);
            var preOpenYear = await GetYearAsync(YearStatus.Preopen, teacher.BusinessId, cancellationToken);

            TeacherProfileDetailsDto detailsDto = GetTeacherDetailsDto(teacher, teacherUser);
            TeacherContactDetailsDto contactDetailsDto = GetTeacherContactDetailsDto(teacher, teacherUser);

            var penddingRequest = await _studentTeacherRequestRepo.GetBySpecAsync(new PenddingRequestByStudentIdAndTeacherIdSpec(teacher.Id, student.Id), cancellationToken);
            var studentTeacherCommunication = await _studentTeacherCommunicationRepo.GetBySpecAsync(new StudentTeacherCommunicationByStudentIdAndTeacherIdSpec(student.Id, teacher.Id), cancellationToken);

            return new()
            {
                Id = request.Id,
                Details = detailsDto,
                ContactDetails = contactDetailsDto,
                Summary = teacher.Summary,
                Headquarters = teacher.HeadQuarters.Adapt<List<TeacherHeadquartersDetailsDto>>(),
                OpenYear = GetTeacherTuitionYearDetailsDto(openYear),
                PreOpenYear = GetTeacherTuitionYearDetailsDto(preOpenYear),
                Teachers = exploreTeacherDtos,
                CanSendRequest = studentTeacherCommunication == null ? true : studentTeacherCommunication.CanSendAgain,
                RequestId = penddingRequest?.Id,
            };
        }

        private async Task<List<ExploreTeacherDto>> GetExploreTeacherDtos(List<Teacher>? exploreTeachers, CancellationToken cancellationToken)
        {
            List<ExploreTeacherDto> exploreTeacherDtos = new();
            if(exploreTeachers != null)
            {
                foreach (var exploreTeacher in exploreTeachers)
                {
                    var teacherFound = await _userService.GetAsync(exploreTeacher.Id.ToString(), cancellationToken);
                    ExploreTeacherDto exploreTeacherDto = new()
                    {
                        Id = exploreTeacher.Id,
                        Name = teacherFound.FirstName + " " + teacherFound.LastName,
                        Image = exploreTeacher.ImagePath,
                        CourseId = exploreTeacher.CourseId,
                        Course = exploreTeacher.Course.Name
                    };
                    exploreTeacherDtos.Add(exploreTeacherDto);
                }
            }

            return exploreTeacherDtos;
        }

        private async Task<Teacher> GetTeacherAsync(Guid teacherId, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepo.GetBySpecAsync(new TeacherByTeacherIdIncludeDetailsSpec(teacherId), cancellationToken);
            _ = teacher ?? throw new NotFoundException(_t["Teacher {0} Not Found!", teacherId]);
            return teacher;
        }

        private async Task<Year?> GetYearAsync(string state, Guid businessId, CancellationToken cancellationToken)
        {
            return await _yearRepo.GetBySpecAsync(
                new YearsForExploreTeacherByBusinessIdAndStateIncludeTeacherCourseLevelYearsSpec(
                    state,
                    businessId),
                cancellationToken);
        }

        private async Task<Student?> GetStudentAsync(CancellationToken cancellationToken)
        {
            return await _studentRepo.GetBySpecAsync(new GetLevelByStudentIdSpec(_currentUser.GetUserId()), cancellationToken);
        }
        
        private static TeacherProfileDetailsDto GetTeacherDetailsDto(Teacher teacher, UserDetailsDto? user)
        {
            return new TeacherProfileDetailsDto
            {
                Name = $"{user!.FirstName} {user.LastName}",
                Image = teacher.ImagePath,
                CourseId = teacher.CourseId,
                Course = teacher.Course.Name,
                BirthDay = teacher.BirthDay,
                Address = teacher.Address,
                Nationality = teacher.Nationality,
                School = teacher.School,
                Education = teacher.Degree,
                TeachingTypes = teacher.TeachingMethod,
            };
        }

        private static TeacherTuitionYearDetailsDto? GetTeacherTuitionYearDetailsDto(Year? year)
        {
            if (year == null)
                return null;

            TeacherTuitionYearDetailsDto dto = new() 
            { 
                Id = year.Id,
                State = year.DashboardYear.State,
                Start = year.DashboardYear.Start,
                End = year.DashboardYear.Start + 1,
            };
            if(year.TeacherCourseLevelYears != null)
            {
                List<TeacherTuitionYearLevelDetailsDto> LevelDtos = new();
                foreach(var teacherCourseLevelYear in year.TeacherCourseLevelYears)
                {
                    TeacherTuitionYearLevelDetailsDto levelDto = new() 
                    { 
                        LevelName = teacherCourseLevelYear.TeacherCourseLevel.Level.Name,
                        MonthlyFees = teacherCourseLevelYear.MonthFee,
                        DownPayment = teacherCourseLevelYear.IntroFee
                    };
                    LevelDtos.Add(levelDto);
                }
                dto.Levels = LevelDtos;
            }

            return dto;
        }

        private static TeacherContactDetailsDto GetTeacherContactDetailsDto(Teacher teacher, UserDetailsDto? user)
        {
            var contact = teacher.TeacherContacts?.FirstOrDefault(e => e.Contact == Contacts.Facebook);
            return new TeacherContactDetailsDto
            {
                Phone = user!.PhoneNumber,
                Email = user.Email,
                Facebook = contact?.Contact,
                Twitter = teacher.TeacherContacts?.FirstOrDefault(e => e.Contact == Contacts.Twitter)?.Contact,
                Website = teacher.TeacherContacts?.FirstOrDefault(e => e.Contact == Contacts.WebSite)?.Contact,
                Youtube = teacher.TeacherContacts?.FirstOrDefault(e => e.Contact == Contacts.Youtube)?.Contact
            };
        }
       
    }
}
