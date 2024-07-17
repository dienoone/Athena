namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Delete
{
    public record DeleteSectionImageByIdRequest(Guid Id) : IRequest<Guid>;
    public class DeleteSectionImageByIdRequestHandler : IRequestHandler<DeleteSectionImageByIdRequest, Guid>
    {
        private readonly IRepository<SectionImage> _sectionImageRepo;
        private readonly IStringLocalizer<DeleteSectionImageByIdRequestHandler> _t;
        private readonly IFileStorageService _file;

        public DeleteSectionImageByIdRequestHandler(IRepository<SectionImage> sectionImageRepo,
            IStringLocalizer<DeleteSectionImageByIdRequestHandler> t, IFileStorageService file)
        {
            _sectionImageRepo = sectionImageRepo;
            _t = t;
            _file = file;
        }

        public async Task<Guid> Handle(DeleteSectionImageByIdRequest request, CancellationToken cancellationToken)
        {
            var image = await _sectionImageRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = image ?? throw new NotFoundException(_t["SectionImage {0} Not Found!", request.Id]);
            await DeleteSectionImage(image, cancellationToken);
            return request.Id;
        }

        private async Task DeleteSectionImage(SectionImage sectionImage, CancellationToken ct)
        {
            DeleteImage(sectionImage.Image);
            await _sectionImageRepo.DeleteAsync(sectionImage, ct);

        }

        private void DeleteImage(string? currentImagePath)
        {
            if (!string.IsNullOrEmpty(currentImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentImagePath));
            }
        }
    }


}
