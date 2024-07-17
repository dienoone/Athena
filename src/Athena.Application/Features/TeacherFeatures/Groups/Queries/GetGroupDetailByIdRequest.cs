using Athena.Application.Features.TeacherFeatures.Groups.Dtos;
using Athena.Application.Features.TeacherFeatures.Groups.Spec;

namespace Athena.Application.Features.TeacherFeatures.Groups.Queries
{
    // Todo: Need To Add List Of Students To Dto
    public record GetGroupDetailByIdRequest(Guid Id) : IRequest<GroupDetailDto>;

    public class GetGroupDetailByIdRequestHandler : IRequestHandler<GetGroupDetailByIdRequest, GroupDetailDto>
    {
        private readonly IStringLocalizer<GetGroupDetailByIdRequestHandler> _t;
        private readonly IReadRepository<Group> _groupRepo;
        private readonly IReadRepository<Student> _studentRepo;

        public GetGroupDetailByIdRequestHandler(IStringLocalizer<GetGroupDetailByIdRequestHandler> t, IReadRepository<Group> groupRepo, 
            IReadRepository<Student> studentRepo) =>
            (_t, _groupRepo, _studentRepo) = (t, groupRepo, studentRepo);

        public async Task<GroupDetailDto> Handle(GetGroupDetailByIdRequest request, CancellationToken cancellationToken)
        {
            var group = await _groupRepo.GetBySpecAsync(new GroupDetailByIdSpec(request.Id), cancellationToken);
            _ = group ?? throw new NotFoundException(_t["Group {0} Not Found!", request.Id]);
            var result = group.Adapt<GroupDetailDto>();

            if(group.GroupStudents != null)
            {
                List<GroupStudentDto> dtos = new();
                foreach(var student in group.GroupStudents)
                {
                    var std = await _studentRepo.GetBySpecAsync(new StudentWithLevelByStudentIdSpec(student.TeacherCourseLevelYearStudent.StudentId), cancellationToken);
                    GroupStudentDto dto = new()
                    {
                        Id = student.TeacherCourseLevelYearStudent.StudentId,
                        Image = std!.Image,
                        FullName = std.Name,
                        Level = std.LevelClassification.Level.Name,
                        Code = std.Code
                    };
                    dtos.Add(dto);
                }
                result.GroupStudents = dtos;
            }

            return result;
        }
    }
}
