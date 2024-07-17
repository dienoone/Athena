namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Delete
{
    public record DeleteQuestionChoiceByIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteQuestionChoiceByIdRequestHandler : IRequestHandler<DeleteQuestionChoiceByIdRequest, Guid>
    {
        private readonly IRepository<QuestionChoice> _questionChoiceRepo;
        private readonly IStringLocalizer<DeleteQuestionChoiceByIdRequestHandler> _t;
        private readonly IFileStorageService _file;

        public DeleteQuestionChoiceByIdRequestHandler(IRepository<QuestionChoice> questionChoiceRepo,
            IStringLocalizer<DeleteQuestionChoiceByIdRequestHandler> t, IFileStorageService file)
        {
            _questionChoiceRepo = questionChoiceRepo;
            _t = t;
            _file = file;
        }

        public async Task<Guid> Handle(DeleteQuestionChoiceByIdRequest request, CancellationToken cancellationToken)
        {
            var choice = await _questionChoiceRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = choice ?? throw new NotFoundException(_t["QuestionChoice {0} Not Found!", request.Id]);
            await DeleteQuestionChoice(choice, cancellationToken);
            return request.Id;
        }

        private async Task DeleteQuestionChoice(QuestionChoice choice, CancellationToken cancellationToken)
        {

            DeleteImage(choice.Image);
            await _questionChoiceRepo.DeleteAsync(choice, cancellationToken);
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
