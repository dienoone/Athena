using Athena.Application.Features.StudentFeatures.Students.Dtos;
using Athena.Application.Identity.Users;
using Athena.Domain.Common.Const;
using Athena.Shared.Authorization;
using System.Text.Json.Serialization;

namespace Athena.Application.Features.StudentFeatures.Students
{
    // If User Enter UnRespectValues Of SomeError Phones:
    // We Need To Check That Things From Teacher By Making Him Report That Students
    // And It Will Send Email To Us And We Will Recevie That And Act Upon it
    // TODO: That Thing
    // TODO: Send token here
    public class CreateStudentRequest : IRequest<CreateStudnetResponseDto>
    {
        public CreateUserRequest CreateUser { get; set; } = null!;
        public FileUploadRequest? Image { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDay { get; set; }
        public string ParentName { get; set; } = default!;
        public string ParentJob { get; set; } = default!;
        public string ParentPhone { get; set; } = default!;
        public string HomePhone { get; set; } = default!;
        public string School { get; set; } = default!;
        public Guid LevelClassificationId { get; set; }

        [JsonIgnore]
        public string? Origin { get; set; }
    }

    public class CreateStudentRequestHandler : IRequestHandler<CreateStudentRequest, CreateStudnetResponseDto>
    {
        private readonly IRepository<Student> _studentRepo;
        private readonly IRepository<CodeHelper> _codeHelperRepo;
        private readonly IStringLocalizer<CreateStudentRequestHandler> _t;
        private readonly IFileStorageService _file;
        private readonly IUserService _userSevice;

        public CreateStudentRequestHandler(
            IRepository<Student> studentRepo, 
            IRepository<CodeHelper> codeHelperRepo, 
            IStringLocalizer<CreateStudentRequestHandler> t, 
            IFileStorageService file, 
            IUserService userSevice)
        {
            _studentRepo = studentRepo;
            _codeHelperRepo = codeHelperRepo;
            _t = t;
            _file = file;
            _userSevice = userSevice;
        }

        public async Task<CreateStudnetResponseDto> Handle(CreateStudentRequest request, CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(await _userSevice.CreateAsync(request.CreateUser, request.Origin!, ARoles.Student, null));
            // TODO: Generate Code:
            string code = "#" + await GenerateCode(cancellationToken);

            string imagePath = string.Empty;
            if (request.Image != null)
                imagePath = await _file.UploadAsync<Student>(request.Image, FileType.Image, cancellationToken);

            var studentName = request.CreateUser.FirstName + " " + request.CreateUser.MiddleName + " " + request.CreateUser.LastName;

            // Create Student:
            Student newStudent = new(id,
                studentName,
                request.CreateUser.Gender,
                request.Address,
                imagePath,
                request.BirthDay,
                request.ParentName,
                request.ParentJob,
                request.ParentPhone,
                request.HomePhone,
                request.School,
                code,
                request.LevelClassificationId);

            await _studentRepo.AddAsync(newStudent, cancellationToken);

            return new()
            {
                Id = newStudent.Id,
                Code = newStudent.Code
            };
        }

        public async Task<string> GenerateCode(CancellationToken cancellationToken)
        {
            var codeHelper = (await _codeHelperRepo.ListAsync(cancellationToken)).FirstOrDefault();

            if(codeHelper == null)
            {
                CodeHelper newHelper = new(await _studentRepo.CountAsync(cancellationToken));
                await _codeHelperRepo.AddAsync(newHelper, cancellationToken);
            }

            long count = codeHelper!.Count;
            codeHelper.Update(++count);
            await _codeHelperRepo.UpdateAsync(codeHelper, cancellationToken);

            // Calculate the number of digits needed for the random part
            int digitsNeeded = 8 - count.ToString().Length;

            // Generate random part with the appropriate number of digits
            Random random = new();
            string randomPart = random.Next((int)Math.Pow(10, digitsNeeded - 1), (int)Math.Pow(10, digitsNeeded)).ToString(); // Random number with the specified number of digits

            // Combine the fixed part (last 2 digits) and the random part
            string code = randomPart + codeHelper.Count.ToString("D2");

            return code;
        }
    }
}
