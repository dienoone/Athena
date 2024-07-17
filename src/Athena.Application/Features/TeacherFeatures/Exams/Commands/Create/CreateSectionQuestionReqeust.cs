using Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Create
{
    public record CreateSectionQuestionReqeust(Guid SectionId, CreateExamRequestQuestionHelper NewQuestion) : IRequest<Guid>;

    public class CreateSectionQuestionReqeustValidator : CustomValidator<CreateSectionQuestionReqeust>
    {
        public CreateSectionQuestionReqeustValidator()
        {
            RuleFor(e => e.NewQuestion)
                .InjectValidator();
        }
    }

    public class CreateSectionQuestionReqeustHandler : IRequestHandler<CreateSectionQuestionReqeust, Guid>
    {
        private readonly IRepository<Exam> _examRepo;
        private readonly IRepository<Section> _sectionRepo;
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<QuestionImage> _questionImageRepo;
        private readonly IRepository<QuestionChoice> _questionChoiceRepo;
        private readonly IStringLocalizer<CreateSectionQuestionReqeustHandler> _t;
        private readonly IFileStorageService _file;

        public CreateSectionQuestionReqeustHandler(
            IRepository<Exam> examRepo,
            IRepository<Section> sectionRepo,
            IRepository<Question> questionRepo,
            IRepository<QuestionImage> questionImageRepo,
            IRepository<QuestionChoice> questionChoiceRepo,
            IFileStorageService file,
            IStringLocalizer<CreateSectionQuestionReqeustHandler> t)
        {
            _examRepo = examRepo;
            _sectionRepo = sectionRepo;
            _questionRepo = questionRepo;
            _questionImageRepo = questionImageRepo;
            _questionChoiceRepo = questionChoiceRepo;
            _file = file;
            _t = t;
        }

        public async Task<Guid> Handle(CreateSectionQuestionReqeust request, CancellationToken cancellationToken)
        {
            var section = await _sectionRepo.GetByIdAsync(request.SectionId, cancellationToken);
            _ = section ?? throw new NotFoundException(_t["Section {0} Not Found!", request.SectionId]);

            await CreateNewQuestion(request.NewQuestion, request.SectionId, section.BusinessId, cancellationToken);
            await UpdateSectionAndExamDegree(section.Id, cancellationToken);
            return section.Id;
        }

        #region Helpers:

        private async Task CreateNewQuestion(CreateExamRequestQuestionHelper question, Guid sectionId, Guid busniessId, CancellationToken cancellationToken)
        {
            Question newQuestion = new(question.Index, question.Name, question.Type, question.Answer, question.Degree, question.IsPrime, sectionId, busniessId);
            await _questionRepo.AddAsync(newQuestion, cancellationToken);

            if (question.Images != null && question.Images.Count > 0)
            {
                foreach (var image in question.Images)
                {
                    await CreateQuestionImage(image, newQuestion.Id, busniessId, cancellationToken);
                }
            }

            if (question.Type == QuestionTypes.MCQ)
            {
                if (question.Choices != null && question.Choices.Count > 0)
                {
                    foreach (CreateExamRequestQuestionChoicesHelper choice in question.Choices)
                    {
                        await CreateQuestionChoice(choice, newQuestion.Id, busniessId, cancellationToken);
                    }
                }
            }
        }

        private async Task CreateQuestionImage(CreateExamRequestImageHelper image, Guid questionId, Guid businessId, CancellationToken cancellationToken)
        {
            string imagePaht = await _file.UploadAsync<QuestionImage>(image.Image, FileType.Image, cancellationToken);
            QuestionImage newQuestionImage = new(imagePaht, image.Index, questionId, businessId);
            await _questionImageRepo.AddAsync(newQuestionImage, cancellationToken);
        }

        private async Task CreateQuestionChoice(CreateExamRequestQuestionChoicesHelper choice, Guid questionId, Guid businessId, CancellationToken cancellationToken)
        {
            string image = string.Empty;
            if (choice.Image != null)
            {
                image = await _file.UploadAsync<QuestionChoice>(choice.Image, FileType.Image, cancellationToken);
            }

            QuestionChoice newQuestionChoice = new(choice.Index, choice.Name, image, choice.IsRightChoice, questionId, businessId);
            await _questionChoiceRepo.AddAsync(newQuestionChoice, cancellationToken);
        }

        private async Task UpdateSectionAndExamDegree(Guid sectionId, CancellationToken cancellationToken)
        {
            var section = await _sectionRepo.GetByIdAsync(sectionId, cancellationToken);
            var questions = await _questionRepo.ListAsync(new QuestionsBySectionIdSpec(sectionId), cancellationToken);

            double sectionDegree = questions.Sum(e => e.Degree);
            section!.Update(null, null, null, sectionDegree, null, null);
            await _sectionRepo.UpdateAsync(section, cancellationToken);
            await UpdateExamDegree(section.ExamId, cancellationToken);
        }

        private async Task UpdateExamDegree(Guid examId, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetByIdAsync(examId, cancellationToken);
            var questions = await _questionRepo.ListAsync(new QuestionsByExamIdSpec(examId), cancellationToken);

            double examDegree = questions.Sum(e => e.Degree);
            exam!.Update(null, null, null, examDegree, null, null, null, null, null, null, null);

        }

        #endregion
    }
}
