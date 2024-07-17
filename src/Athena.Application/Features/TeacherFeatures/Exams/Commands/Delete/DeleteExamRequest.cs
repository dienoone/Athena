using Athena.Application.Features.TeacherFeatures.Exams.Spec;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Delete
{
    public record DeleteExamRequest(Guid Id) : IRequest<Guid>;

    public class DeleteExamRequestHandler : IRequestHandler<DeleteExamRequest, Guid>
    {
        private readonly IRepository<Exam> _examRepo;
        private readonly IRepository<Section> _sectionRepo;
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<SectionImage> _sectionImageRepo;
        private readonly IRepository<QuestionChoice> _questionChoiceRepo;
        private readonly IRepository<QuestionImage> _questionImageRepo;
        private readonly IFileStorageService _file;
        private readonly IStringLocalizer<DeleteExamRequestHandler> _t;

        public DeleteExamRequestHandler(IRepository<Exam> examRepo, IRepository<Section> sectionRepo,
            IRepository<Question> questionRepo, IRepository<SectionImage> sectionImageRepo,
            IRepository<QuestionImage> questionImageRepo, IRepository<QuestionChoice> questionChoiceRepo,
            IFileStorageService file, IStringLocalizer<DeleteExamRequestHandler> t)
        {
            _examRepo = examRepo;
            _sectionRepo = sectionRepo;
            _questionRepo = questionRepo;
            _questionChoiceRepo = questionChoiceRepo;
            _sectionImageRepo = sectionImageRepo;
            _questionImageRepo = questionImageRepo;
            _file = file;
            _t = t;
        }

        public async Task<Guid> Handle(DeleteExamRequest request, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", request.Id]);

            var sections = await _sectionRepo.ListAsync(new SectionsDetailByExamIdSpec(exam.Id), cancellationToken);
            foreach (var section in sections)
            {
                var questions = await _questionRepo.ListAsync(new QuestionsDetailBySectionIdSpec(section.Id), cancellationToken);
                foreach (var question in questions)
                {
                    await DeleteQuestion(question, cancellationToken);
                }

                if (section.SectionImages != null)
                {
                    foreach (var image in section.SectionImages)
                    {
                        await DeleteSectionImage(image, cancellationToken);
                    }
                }

                await _sectionRepo.DeleteAsync(section, cancellationToken);
            }

            await _examRepo.DeleteAsync(exam, cancellationToken);
            return exam.Id;
        }

        #region Helpers:

        private async Task DeleteSectionImage(SectionImage sectionImage, CancellationToken ct)
        {
            DeleteImage(sectionImage.Image);
            await _sectionImageRepo.DeleteAsync(sectionImage, ct);

        }

        private async Task DeleteQuestion(Question question, CancellationToken cancellationToken)
        {
            if (question.QuestionChoices != null)
            {
                foreach (QuestionChoice choice in question.QuestionChoices)
                {
                    await DeleteQuestionChoice(choice, cancellationToken);
                }
            }

            if (question.QuestionImages != null)
            {
                foreach (QuestionImage image in question.QuestionImages)
                {
                    await DeleteQuestionImage(image, cancellationToken);
                }
            }

            await _questionRepo.DeleteAsync(question, cancellationToken);
        }

        private async Task DeleteQuestionChoice(QuestionChoice choice, CancellationToken cancellationToken)
        {

            DeleteImage(choice.Image);
            await _questionChoiceRepo.DeleteAsync(choice, cancellationToken);
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

        #endregion
    }
}
