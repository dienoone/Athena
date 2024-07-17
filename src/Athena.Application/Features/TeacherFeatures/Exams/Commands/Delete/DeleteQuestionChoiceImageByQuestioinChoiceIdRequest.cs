namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Delete
{
    public record DeleteQuestionChoiceImageByQuestioinChoiceIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteQuestionChoiceImageByQuestioinChoiceIdRequestRequestHandler : IRequestHandler<DeleteQuestionChoiceImageByQuestioinChoiceIdRequest, Guid>
    {
        private readonly IRepository<QuestionChoice> _questionChoiceRepo;
        private readonly IStringLocalizer<DeleteQuestionChoiceImageByQuestioinChoiceIdRequestRequestHandler> _t;
        private readonly IFileStorageService _file;

        public DeleteQuestionChoiceImageByQuestioinChoiceIdRequestRequestHandler(IRepository<QuestionChoice> questionChoiceRepo,
            IStringLocalizer<DeleteQuestionChoiceImageByQuestioinChoiceIdRequestRequestHandler> t, IFileStorageService file)
        {
            _questionChoiceRepo = questionChoiceRepo;
            _t = t;
            _file = file;
        }

        public async Task<Guid> Handle(DeleteQuestionChoiceImageByQuestioinChoiceIdRequest request, CancellationToken cancellationToken)
        {
            var choice = await _questionChoiceRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = choice ?? throw new NotFoundException(_t["QuestionChoice {0} Not Found!", request.Id]);
            DeleteImage(choice.Image);
            choice.ClearImagePath();
            choice.Update(choice.Index, choice.Name, string.Empty, choice.IsRightChoice);
            await _questionChoiceRepo.UpdateAsync(choice, cancellationToken);
            return request.Id;
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
