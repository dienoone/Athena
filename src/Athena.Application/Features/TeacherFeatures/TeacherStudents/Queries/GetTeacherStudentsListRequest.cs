using Athena.Application.Features.DashboardFeatures.Levels.Spec;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec.GroupStudentSpec;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Queries
{
    public record GetTeacherStudentsListRequest() : IRequest<StudentsListRequestDto>;

    public class GetTeacherStudentsListRequestHandler : IRequestHandler<GetTeacherStudentsListRequest, StudentsListRequestDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IReadRepository<Year> _yearRepo;
        private readonly IReadRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;
        private readonly IReadRepository<LevelClassification> _levelClassificationRepo;
        private readonly IReadRepository<GroupStudent> _groupStudentRepo;

        public GetTeacherStudentsListRequestHandler(ICurrentUser currentUser, IUserService userService, IReadRepository<Year> yearRepo,
            IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo, IReadRepository<LevelClassification> levelClassificationRepo,
            IReadRepository<GroupStudent> groupStudentRepo)
        {
            _currentUser = currentUser;
            _userService = userService;
            _yearRepo = yearRepo;
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
            _levelClassificationRepo = levelClassificationRepo;
            _groupStudentRepo = groupStudentRepo;
        }

        public async Task<StudentsListRequestDto> Handle(GetTeacherStudentsListRequest request, CancellationToken cancellationToken)
        {
            List<TeacherStudentYearListDto> dtos = new();
            var openYear = await _yearRepo.GetBySpecAsync(new TeacherStudentListByBusinessIdSpec(_currentUser.GetBusinessId(), YearStatus.Open), cancellationToken);
            var preOpenYear = await _yearRepo.GetBySpecAsync(new TeacherStudentListByBusinessIdSpec(_currentUser.GetBusinessId(), YearStatus.Preopen), cancellationToken);

            var openYearDto = await MapDto(openYear, cancellationToken);
            var preOpenYearDto = await MapDto(preOpenYear, cancellationToken);
            return new() 
            { 
                Open = openYearDto,
                Preopen = preOpenYearDto,
            };
        }

        private async Task<List<TeacherStudentYearListDto>> MapDto(Year? year, CancellationToken cancellationToken)
        {
            if(year == null) return new() { };
            var levels = await _teacherCourseLevelYearRepo.ListAsync(new TeacherCourseLevelYearByYearIdIncludeLevelsSpec(year.Id), cancellationToken);
            List<TeacherStudentYearListDto> teacherStudentYearLevelListDtos = new();

            foreach (var levelWithStudents in levels)
            {
                List<TeacherStudentYearLevelStudentDto> teacherStudentYearLevelStudentDtos = new();
                TeacherStudentYearListDto dto = new() { LevelName = levelWithStudents.TeacherCourseLevel.Level.Name };

                foreach (var student in levelWithStudents.TeacherCourseLevelYearStudents)
                {
                    var levelClassification = await _levelClassificationRepo.GetBySpecAsync(new LevelClassificationDetailByIdSpec(student.Student.LevelClassificationId), cancellationToken);
                    var userDetail = await _userService.GetAsync(student.StudentId.ToString(), cancellationToken);
                    var group = await _groupStudentRepo.GetBySpecAsync(new GroupStudentByStudentIdAndBusinessIdIncludeYearAndGroupSpec(student.StudentId, _currentUser.GetBusinessId()), cancellationToken);

                    TeacherStudentYearLevelStudentDto teacherStudentYearLevelStudentDto = new()
                    {
                        Id = student.Id,
                        ImagePath = student.Student.Image,
                        FullName = student.Student.Name,
                        LevelName = levelClassification!.Level.Name,
                        EducationClassificationName = levelClassification.EducationClassification.Name,
                        GroupName = group!.Group.Name,
                        PhoneNumber = userDetail.PhoneNumber,
                        Email = userDetail.Email,
                        UserName = userDetail.UserName,
                        Code = student.Student.Code
                    };
                    teacherStudentYearLevelStudentDtos.Add(teacherStudentYearLevelStudentDto);
                    
                }
                dto.Students = teacherStudentYearLevelStudentDtos;
                teacherStudentYearLevelListDtos.Add(dto);
            }

            return teacherStudentYearLevelListDtos;
        }
    }


}
