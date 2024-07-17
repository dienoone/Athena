using Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec;
using Athena.Application.Identity.Users;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Queries
{
    // ToDo: Check if the request status is pendding?
    public record GetJoinRequestDetailByRequestIdRequest(Guid Id) : IRequest<JoinRequestDetailDto>;
    
    public class GetJoinRequestDetailByRequestIdRequestHandler : IRequestHandler<GetJoinRequestDetailByRequestIdRequest, JoinRequestDetailDto>
    {
        private readonly IUserService _userService;
        private readonly IReadRepository<StudentTeacherRequest> _studentTeacherRequestRepo;
        private readonly IStringLocalizer<GetJoinRequestDetailByRequestIdRequestHandler> _t;

        public GetJoinRequestDetailByRequestIdRequestHandler(
            IUserService userService, 
            IReadRepository<StudentTeacherRequest> studentTeacherRequestRepo, 
            IStringLocalizer<GetJoinRequestDetailByRequestIdRequestHandler> t)
        {
            _userService = userService;
            _studentTeacherRequestRepo = studentTeacherRequestRepo;
            _t = t;
        }

        public async Task<JoinRequestDetailDto> Handle(GetJoinRequestDetailByRequestIdRequest request, CancellationToken cancellationToken)
        {
            var studentRequest = await _studentTeacherRequestRepo.GetBySpecAsync(new StudentTeacherRequestByIdIncludeStudentAndGroupSpec(request.Id), cancellationToken);
            _ = studentRequest ?? throw new NotFoundException(_t["StudentTeacherRequest {0} Not Found!", request.Id]);

            var userDetail = await _userService.GetAsync(studentRequest.Student.Id.ToString(), cancellationToken);

            return new()
            {
                Id = studentRequest.Id,
                Code = studentRequest.Student.Code,
                FirstName = userDetail.FirstName,
                MiddleName = userDetail.MiddleName,
                LastName = userDetail.LastName,
                UserName = userDetail.UserName,
                LevelName = studentRequest.Student.LevelClassification.Level.Name,
                EducationClassificationName = studentRequest.Student.LevelClassification.EducationClassification.Name,
                Email = userDetail.Email,
                Image = studentRequest.Student.Image,
                Gender = userDetail.Gender,
                BirthDay = studentRequest.Student.BirthDay,
                Address = studentRequest.Student.Address,
                HomePhone = studentRequest.Student.HomePhone,
                ParentName = studentRequest.Student.ParentName,
                ParentJob = studentRequest.Student.ParentJob,
                ParentPhone = studentRequest.Student.ParentPhone,
                Phone = userDetail.PhoneNumber,
                YearState = studentRequest.Group.TeacherCourseLevelYear.Year.DashboardYear.State,
                GroupName = studentRequest.Group.Name
            };
        }
    }
}
