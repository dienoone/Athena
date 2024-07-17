using Athena.Application.Features.StudentFeatures.Students.Dtos;
using Athena.Application.Features.StudentFeatures.Students.Spec;
using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Students
{
    public record GetAllStudentsRequest() : IRequest<List<StudentsListDto>>;

    public class GetAllStudentsRequestHandler : IRequestHandler<GetAllStudentsRequest, List<StudentsListDto>>
    {
        private readonly IUserService _userService;
        private readonly IReadRepository<Student> _studentRepo;

        public GetAllStudentsRequestHandler(IUserService userService, IReadRepository<Student> studentRepo)
        {
            _userService = userService;
            _studentRepo = studentRepo;
        }

        public async Task<List<StudentsListDto>> Handle(GetAllStudentsRequest request, CancellationToken cancellationToken)
        {
            List<StudentsListDto> list = new();
            var students = await _studentRepo.ListAsync(new StudentsListSpec(), cancellationToken);
            foreach(var student in students)
            {
                var userDetail = await _userService.GetAsync(student.Id.ToString(), cancellationToken);
                StudentsListDto dto = new()
                {
                    Id= student.Id,
                    ImagePath = student.Image,
                    FirstName = userDetail.FirstName,
                    MiddleName = userDetail.MiddleName,
                    LastName = userDetail.LastName,
                    LevelName = student.LevelClassification.Level.Name,
                    EducationClassificationName = student.LevelClassification.EducationClassification.Name,
                    PhoneNumber = userDetail.PhoneNumber,
                    Email= userDetail.Email,
                    UserName= userDetail.UserName,
                    Code= student.Code,
                };
                list.Add(dto);

            }
            return list;
        }
    }
}
