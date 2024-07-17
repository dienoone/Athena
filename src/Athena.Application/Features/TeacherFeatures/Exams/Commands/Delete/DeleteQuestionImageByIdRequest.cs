namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Delete
{
    public record DeleteQuestionImageByIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteQuestionImageByIdRequestHandler : IRequestHandler<DeleteQuestionImageByIdRequest, Guid>
    {
        private readonly IRepository<QuestionImage> _questionImageRepo;
        private readonly IStringLocalizer<DeleteQuestionImageByIdRequestHandler> _t;
        private readonly IFileStorageService _file;

        public DeleteQuestionImageByIdRequestHandler(IRepository<QuestionImage> questionImageRepo,
            IStringLocalizer<DeleteQuestionImageByIdRequestHandler> t, IFileStorageService file)
        {
            _questionImageRepo = questionImageRepo;
            _t = t;
            _file = file;
        }

        public async Task<Guid> Handle(DeleteQuestionImageByIdRequest request, CancellationToken cancellationToken)
        {
            var image = await _questionImageRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = image ?? throw new NotFoundException(_t["QuestionImage {0} Not Found!", request.Id]);
            await DeleteQuestionImage(image, cancellationToken);
            return request.Id;
        }

        private async Task DeleteQuestionImage(QuestionImage questionImage, CancellationToken ct)
        {
            DeleteImage(questionImage.Image);
            await _questionImageRepo.DeleteAsync(questionImage, ct);
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
