using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Profile.Commands
{
    public record UpdateStudentImageRequest(FileUploadRequest Image) : IRequest<Guid>;

    public class UpdateStudentImageRequestValidator : CustomValidator<UpdateStudentImageRequest> 
    {
        public UpdateStudentImageRequestValidator()
        {
            RuleFor(p => p.Image)
               .InjectValidator();
        }
    }

    public class UpdateStudentImageRequestHandler : IRequestHandler<UpdateStudentImageRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;
        private readonly IRepository<Student> _studentRepo;

        public UpdateStudentImageRequestHandler(ICurrentUser currentUser, IFileStorageService fileStorageService,
            IRepository<Student> studentRepo)
        {
            _currentUser = currentUser;
            _studentRepo = studentRepo;
            _fileStorageService = fileStorageService;
        }

        public async Task<Guid> Handle(UpdateStudentImageRequest request, CancellationToken cancellationToken)
        {
            var student = await _studentRepo.GetByIdAsync(_currentUser.GetUserId(), cancellationToken);

            string? currentStudentImagePath = student!.Image;
            if (!string.IsNullOrEmpty(currentStudentImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _fileStorageService.Remove(Path.Combine(root, currentStudentImagePath));
            }

            string? studentImagePath = request.Image is not null
            ? await _fileStorageService.UploadAsync<Student>(request.Image, FileType.Image, cancellationToken)
            : null;

            student.Update(null, null, null, studentImagePath, null, null, null, null, null, null, null);
            await _studentRepo.UpdateAsync(student, cancellationToken);
            return student.Id;
        }
    }

}
