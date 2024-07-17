using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;
using Athena.Shared.Authorization;
using System.Text.Json.Serialization;

namespace Athena.Application.Features.DashboardFeatures.Teachers.Commands
{
    // BusinessId: TeacherId
    public class CreateTeacherRequest : IRequest<Guid>
    {
        public CreateUserRequest User { get; set; } = null!;
        public FileUploadRequest? Image { get; set; }
        public string? Address { get; set; }
        public Guid CourseId { get; set; }
        public List<Guid> LevelIds { get; set; } = new();


        [JsonIgnore]
        public string? Origin { get; set; }
    }

    public class CreateTeacherRequestHandler : IRequestHandler<CreateTeacherRequest, Guid>
    {
        private readonly IRepository<Teacher> _teacherRepo;
        private readonly IRepository<TeacherCourseLevel> _teacherCourseLevelRepo;
        private readonly IStringLocalizer<CreateTeacherRequestHandler> _t;
        private readonly IFileStorageService _file;
        private readonly IUserService _userSevice;

        public CreateTeacherRequestHandler(
            IRepository<Teacher> teacherRepo,
            IRepository<TeacherCourseLevel> teacherCourseLevelRepo,
            IStringLocalizer<CreateTeacherRequestHandler> stringLocalizer,
            IFileStorageService file,
            IUserService userSevice)
        {
            _teacherRepo = teacherRepo;
            _teacherCourseLevelRepo = teacherCourseLevelRepo;
            _t = stringLocalizer;
            _file = file;
            _userSevice = userSevice;
        }

        public async Task<Guid> Handle(CreateTeacherRequest request, CancellationToken cancellationToken)
        {
            if (!(request.LevelIds.Count > 0))
            {
                throw new InternalServerException(_t["Level's can't be null."]);
            }

            Guid id = Guid.Parse(await _userSevice.CreateAsync(request.User, request.Origin!, ARoles.Teacher, null));

            // Create Teacher:
            string imagePaht = string.Empty;
            if (request.Image != null)
                imagePaht = await _file.UploadAsync<Teacher>(request.Image, FileType.Image, cancellationToken);

            var teacherName = request.User.FirstName + " " + request.User.MiddleName + " " + request.User.LastName;

            Teacher newTeacher = new(id, teacherName, request.User.Gender, request.Address, imagePaht, null, null, null, null, null, null, null, request.CourseId, id);
            await _teacherRepo.AddAsync(newTeacher, cancellationToken);

            //  Create TeacherCourseLevels:
            foreach (var level in request.LevelIds)
            {
                TeacherCourseLevel teacherCourseLevel = new(id, level, id);
                await _teacherCourseLevelRepo.AddAsync(teacherCourseLevel, cancellationToken);
            }

            return id;
        }
    }
}
