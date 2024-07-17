using Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Update
{
    // UpdateExam, basicData, Images for secion only
    // ToDo: Review All Validations:
    public class UpdateExamSectionRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public int? Index { get; set; }
        public string? Name { get; set; }
        public string? Paragraph { get; set; }
        public bool? IsPrime { get; set; }
        public int? Time { get; set; }

        public List<UpdateSectionRequestSectionImageHelper>? Images { get; set; }
        public List<CreateExamRequestImageHelper>? NewImages { get; set; }
    }

    public class UpdateExamSectionRequestHandler : IRequestHandler<UpdateExamSectionRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Section> _sectionRepo;
        private readonly IRepository<SectionImage> _sectionImageRepo;
        private readonly IFileStorageService _file;

        public UpdateExamSectionRequestHandler(
            ICurrentUser currentUser,
            IRepository<Section> sectionRepo,
            IRepository<SectionImage> sectionImageRepo,
            IFileStorageService file)
        {
            _currentUser = currentUser;
            _sectionRepo = sectionRepo;
            _sectionImageRepo = sectionImageRepo;
            _file = file;
        }

        public async Task<Guid> Handle(UpdateExamSectionRequest request, CancellationToken cancellationToken)
        {
            Guid buinessId = _currentUser.GetBusinessId();
            var section = await _sectionRepo.GetByIdAsync(request.Id, cancellationToken);

            #region Images:

            if (request.NewImages != null)
            {
                foreach (var image in request.NewImages)
                {
                    await CreateSectionImage(image, section!.Id, buinessId, cancellationToken);
                }
            }

            if (request.Images != null)
            {
                foreach (var image in request.Images)
                {
                    var queryImage = await _sectionImageRepo.GetByIdAsync(image.Id, cancellationToken);
                    await UpdateSectionImage(image, queryImage!, cancellationToken);
                }
            }

            #endregion

            section!.Update(request.Index, request.Name, request.Paragraph, null, request.IsPrime, null);
            await _sectionRepo.UpdateAsync(section, cancellationToken);

            return section.Id;
        }

        #region Helpers :

        #region Section:

        private async Task CreateSectionImage(CreateExamRequestImageHelper image, Guid sectionId, Guid businessId, CancellationToken cancellationToken)
        {
            string imagePaht = await _file.UploadAsync<SectionImage>(image.Image, FileType.Image, cancellationToken);
            SectionImage newSectionImage = new(imagePaht, image.Index, sectionId, businessId);
            await _sectionImageRepo.AddAsync(newSectionImage, cancellationToken);
        }

        private async Task UpdateSectionImage(UpdateSectionRequestSectionImageHelper image, SectionImage queryImage, CancellationToken cancellationToken)
        {
            string? sectionImagePath = image!.Image is not null
                ? await _file.UploadAsync<SectionImage>(image.Image, FileType.Image, cancellationToken)
                : null;

            var updatedSectionImage = queryImage!.Update(sectionImagePath, image.Index);
            await _sectionImageRepo.UpdateAsync(updatedSectionImage, cancellationToken);
        }

        #endregion

        #endregion

    }
}
