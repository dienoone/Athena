using Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Update
{
    public class UpdateQuestionByQuestionIdRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public int? Index { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Answer { get; set; }
        public double? Degree { get; set; }
        public bool? IsPrime { get; set; }

        public List<UpdateSectionRequestQuestionChoiceHelper>? Choices { get; set; }
        public List<UpdateSectionRequestQuestionImageHelper>? Images { get; set; }

        public List<CreateExamRequestImageHelper>? NewImages { get; set; }
        public List<CreateExamRequestQuestionChoicesHelper>? NewChoices { get; set; }
    }



    public class UpdateQuestionByQuestionIdRequestHandler : IRequestHandler<UpdateQuestionByQuestionIdRequest, Guid>
    {
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<QuestionImage> _questionImageRepo;
        private readonly IRepository<QuestionChoice> _questionChoiceRepo;
        private readonly IRepository<Exam> _examRepo;
        private readonly IRepository<Section> _sectionRepo;
        private readonly IFileStorageService _file;
        private readonly IStringLocalizer<UpdateQuestionByQuestionIdRequestHandler> _t;

        public UpdateQuestionByQuestionIdRequestHandler(
            IRepository<Question> questionRepo,
            IRepository<QuestionImage> questionImageRepo,
            IRepository<QuestionChoice> questionChoiceRepo,
            IRepository<Exam> examRepo,
            IRepository<Section> sectionRepo,
            IFileStorageService file,
            IStringLocalizer<UpdateQuestionByQuestionIdRequestHandler> t)
        {
            _questionRepo = questionRepo;
            _questionImageRepo = questionImageRepo;
            _questionChoiceRepo = questionChoiceRepo;
            _examRepo = examRepo;
            _sectionRepo = sectionRepo;
            _file = file;
            _t = t;
        }

        public async Task<Guid> Handle(UpdateQuestionByQuestionIdRequest request, CancellationToken cancellationToken)
        {
            var queryQuestion = await _questionRepo.GetByIdAsync(request.Id, cancellationToken);

            if (request.Type != null)
            {
                if (!(request.Type == QuestionTypes.MCQ || request.Type == QuestionTypes.Written))
                    throw new ConflictException(_t["Invalied QuestionType"]);
            }

            if (request.Degree != null)
            {
                if (!(request.Degree >= 0))
                    throw new ConflictException(_t["Question Degree Must be greater than or equal 0"]);
            }

            await UpdateQuestion(request, queryQuestion!, queryQuestion!.BusinessId, cancellationToken);
            await UpdateSectionAndExamDegree(queryQuestion.SectionId, cancellationToken);
            return request.Id;
        }

        private async Task UpdateQuestion(UpdateQuestionByQuestionIdRequest question, Question queryQuestion, Guid businessId, CancellationToken cancellationToken)
        {
            #region Images:

            // Create:
            if (question.NewImages != null)
            {
                foreach (var image in question.NewImages)
                {
                    await CreateQuestionImage(image, queryQuestion!.Id, businessId, cancellationToken);
                }
            }

            if (question.Images != null)
            {
                foreach (var image in question.Images)
                {
                    var queryImage = await _questionImageRepo.GetByIdAsync(image.Id, cancellationToken);
                    await UpdateQuestionImage(image, queryImage!, cancellationToken);
                }
            }


            #endregion

            #region Choices:

            // Create:
            if (question.NewChoices != null)
            {
                foreach (var choice in question.NewChoices)
                {
                    await CreateQuestionChoice(choice, queryQuestion!.Id, businessId, cancellationToken);
                }
            }

            if (question.Choices != null)
            {
                foreach (var choice in question.Choices)
                {
                    var queryChoice = await _questionChoiceRepo.GetByIdAsync(choice.Id, cancellationToken);
                    await UpdateQuestionChoice(choice, queryChoice!, cancellationToken);
                }
            }

            #endregion

            queryQuestion!.Update(question.Index, question.Name, question.Type, question.Answer, question.Degree, question.IsPrime);
            await _questionRepo.UpdateAsync(queryQuestion, cancellationToken);
        }

        private async Task UpdateQuestionChoice(UpdateSectionRequestQuestionChoiceHelper choice, QuestionChoice queryChoice, CancellationToken cancellationToken)
        {
            string? queryChoiceImagePath = queryChoice!.Image is not null
                ? await _file.UploadAsync<QuestionChoice>(choice.Image, FileType.Image, cancellationToken)
                : null;

            var updatedChoice = queryChoice.Update(choice.Index, choice.Name, queryChoiceImagePath, choice.IsRightChoice);
            await _questionChoiceRepo.UpdateAsync(updatedChoice, cancellationToken);
        }

        private async Task UpdateQuestionImage(UpdateSectionRequestQuestionImageHelper image, QuestionImage queryImage, CancellationToken cancellationToken)
        {
            string? questionImagePath = image!.Image is not null
                ? await _file.UploadAsync<QuestionImage>(image.Image, FileType.Image, cancellationToken)
                : null;

            var updatedQuestionImage = queryImage!.Update(questionImagePath, image.Index);
            await _questionImageRepo.UpdateAsync(updatedQuestionImage, cancellationToken);
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
            await _examRepo.UpdateAsync(exam, cancellationToken);
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
