using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Students
{
    public class UpdateStudentRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public UpdateUserRequest User { get; set; } = null!;
        public string? Address { get; set; }
        public string ParentName { get; set; } = default!;
        public string ParentJob { get; set; } = default!;
        public string ParentPhone { get; set; } = default!;
        public string HomePhone { get; set; } = default!;
        public Guid LevelId { get; set; }

        public UpdateStudentRequest(Guid id) => Id = id;
    }

    public class UpdateStudentRequestHandler : IRequestHandler<UpdateStudentRequest, Guid>
    {
        private readonly IUserService _userService;
        private readonly IReadRepository<Level> _levelRepo;
        private readonly IRepository<Student> _studentRepo;
        private readonly IStringLocalizer<UpdateStudentRequestHandler> _t;

        public UpdateStudentRequestHandler(IUserService userService, IReadRepository<Level> levelRepo,
            IRepository<Student> studentRepo, IStringLocalizer<UpdateStudentRequestHandler> t) =>
            (_userService, _levelRepo, _studentRepo, _t) = (userService, levelRepo, studentRepo, t);

        public async Task<Guid> Handle(UpdateStudentRequest request, CancellationToken cancellationToken)
        {
            var level = await _levelRepo.GetByIdAsync(request.LevelId, cancellationToken);
            _ = level ?? throw new NotFoundException(_t["Level {0} Not Found.", request.LevelId]);

            var student = await _studentRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = student ?? throw new NotFoundException(_t["Student {0} Not Found.", request.Id]);

            await _userService.UpdateAsync(request.User, request.Id.ToString());

            /* student.Update(request.User.FirstName + " " + request.User.LastName,
               request.Address, request.ParentName, request.ParentJob, request.ParentPhone, request.HomePhone, level.Id);*/

            await _studentRepo.UpdateAsync(student, cancellationToken);

            return student.Id;
        }
    }
}
