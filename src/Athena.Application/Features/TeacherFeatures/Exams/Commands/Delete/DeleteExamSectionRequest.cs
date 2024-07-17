using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Delete
{
    public record DeleteExamSectionRequest(Guid Id) : IRequest<Guid>;
    public class DeleteExamSectionRequestHandler : IRequestHandler<DeleteExamSectionRequest, Guid>
    {
        private readonly IRepository<Section> _sectionRepo;
        private readonly IRepository<SectionImage> _sectionImageRepo;
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<QuestionChoice> _questionChoiceRepo;
        private readonly IRepository<QuestionImage> _questionImageRepo;
        private readonly IFileStorageService _file;
        private readonly IStringLocalizer<DeleteExamSectionRequestHandler> _t;

        public DeleteExamSectionRequestHandler(IRepository<Section> sectionRepo, IRepository<SectionImage> sectionImageRepo,
           IRepository<Question> questionRepo, IRepository<QuestionChoice> questionChoiceRepo, IRepository<QuestionImage> questionImageRepo,
           IFileStorageService file, IStringLocalizer<DeleteExamSectionRequestHandler> t)
        {
            _sectionRepo = sectionRepo;
            _sectionImageRepo = sectionImageRepo;
            _questionRepo = questionRepo;
            _questionChoiceRepo = questionChoiceRepo;
            _questionImageRepo = questionImageRepo;
            _file = file;
            _t = t;
        }

        public async Task<Guid> Handle(DeleteExamSectionRequest request, CancellationToken cancellationToken)
        {
            var section = await _sectionRepo.GetBySpecAsync(new SectionsDetailBySectionIdSpec(request.Id), cancellationToken);
            _ = section ?? throw new NotFoundException(_t["Section {0} Not Found!", request.Id]);

            await DeleteSectionImages(section, cancellationToken);

            var questions = await _questionRepo.ListAsync(new QuestionsDetailBySectionIdSpec(request.Id), cancellationToken);
            await DeleteQuestions(questions, cancellationToken);

            return request.Id;
        }

        private async Task DeleteSectionImages(Section section, CancellationToken cancellationToken)
        {
            if (section.SectionImages != null)
            {
                foreach (var image in section.SectionImages)
                {
                    string root = Directory.GetCurrentDirectory();
                    _file.Remove(Path.Combine(root, image.Image));
                    await _sectionImageRepo.DeleteAsync(image, cancellationToken);
                }
            }
        }

        private async Task DeleteQuestions(List<Question> questions, CancellationToken cancellationToken)
        {
            foreach (var question in questions)
            {
                await DeleteQuestionChoices(question, cancellationToken);
                await DeleteQuestionImages(question, cancellationToken);
                await _questionRepo.DeleteAsync(question, cancellationToken);
            }
        }

        private async Task DeleteQuestionImages(Question question, CancellationToken cancellationToken)
        {
            if (question.QuestionImages != null)
            {
                foreach (var image in question.QuestionImages)
                {
                    string root = Directory.GetCurrentDirectory();
                    _file.Remove(Path.Combine(root, image.Image));
                    await _questionImageRepo.DeleteAsync(image, cancellationToken);
                }
            }
        }

        private async Task DeleteQuestionChoices(Question question, CancellationToken cancellationToken)
        {
            if (question.Type == QuestionTypes.MCQ)
            {
                foreach (var choice in question.QuestionChoices!)
                {
                    string? currentChoiceImagePath = choice!.Image;
                    if (!string.IsNullOrEmpty(currentChoiceImagePath))
                    {
                        string root = Directory.GetCurrentDirectory();
                        _file.Remove(Path.Combine(root, currentChoiceImagePath));
                    }
                    await _questionChoiceRepo.DeleteAsync(choice, cancellationToken);
                }
            }
        }


    }

}
