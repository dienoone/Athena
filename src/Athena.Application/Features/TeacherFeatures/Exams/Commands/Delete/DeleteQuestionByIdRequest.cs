using Athena.Application.Features.TeacherFeatures.Exams.Spec;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Delete
{
    public record DeleteQuestionByIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteQuestionByIdRequestHandler : IRequestHandler<DeleteQuestionByIdRequest, Guid>
    {
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<QuestionImage> _questionImageRepo;
        private readonly IRepository<QuestionChoice> _questionChoiceRepo;
        private readonly IStringLocalizer<DeleteQuestionByIdRequestHandler> _t;
        private readonly IFileStorageService _file;

        public DeleteQuestionByIdRequestHandler(IRepository<Question> questionRepo,
            IRepository<QuestionImage> questionImageRepo, IRepository<QuestionChoice> questionChoiceRepo,
            IStringLocalizer<DeleteQuestionByIdRequestHandler> t, IFileStorageService file)
        {
            _questionRepo = questionRepo;
            _questionImageRepo = questionImageRepo;
            _questionChoiceRepo = questionChoiceRepo;
            _t = t;
            _file = file;
        }

        public async Task<Guid> Handle(DeleteQuestionByIdRequest request, CancellationToken cancellationToken)
        {
            var question = await _questionRepo.GetBySpecAsync(new QuestionDetailsByQuestionIdSpec(request.Id), cancellationToken);
            _ = question ?? throw new NotFoundException(_t["Question {0} Not Found!"]);
            await DeleteQuestion(question, cancellationToken);
            return request.Id;
        }

        private async Task DeleteQuestion(Question question, CancellationToken cancellationToken)
        {
            if (question.QuestionChoices != null)
            {
                foreach (QuestionChoice choice in question.QuestionChoices.Where(e => e.DeletedBy == null))
                {
                    await DeleteQuestionChoice(choice, cancellationToken);
                }
            }

            if (question.QuestionImages != null)
            {
                foreach (QuestionImage image in question.QuestionImages.Where(e => e.DeletedBy == null))
                {
                    await DeleteQuestionImage(image, cancellationToken);
                }
            }

            await _questionRepo.DeleteAsync(question, cancellationToken);
        }

        private async Task DeleteQuestionImage(QuestionImage questionImage, CancellationToken ct)
        {
            DeleteImage(questionImage.Image);
            await _questionImageRepo.DeleteAsync(questionImage, ct);
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
