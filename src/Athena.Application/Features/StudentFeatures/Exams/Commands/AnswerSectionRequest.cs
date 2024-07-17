using Athena.Application.Features.StudentFeatures.Exams.Dtos;
using Athena.Application.Features.StudentFeatures.Exams.Spec;
using Athena.Domain.Common.Const;
using System.Text.Json.Serialization;

namespace Athena.Application.Features.StudentFeatures.Exams.Commands
{
    public record AnswerQuestion(Guid Id, Guid? ChoiceId, string? StudentAnswer);
    
    public class AnswerSectionRequest : IRequest<AnswerActiveSectionDto>
    {
        public Guid SectionId { get; set; }
        public List<AnswerQuestion> Questions { get; set; } = null!;


        [JsonIgnore]
        public Guid UserId { get; set; }
    }

    public class AnswerSectionRequestHandler : IRequestHandler<AnswerSectionRequest, AnswerActiveSectionDto>
    {
        private readonly IReadRepository<Section> _sectionRepo;
        private readonly IReadRepository<QuestionChoice> _questionChoiceRepo;
        private readonly IRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IRepository<ExamStudentAnswer> _examStudentAnswerRepo;
        private readonly IRepository<StudentSectionState> _studentSectionStateRepo;
        private readonly IStringLocalizer<AnswerSectionRequestHandler> _t;

        public AnswerSectionRequestHandler(
            IReadRepository<Section> sectionRepo, 
            IReadRepository<QuestionChoice> questionChoiceRepo, 
            IRepository<ExamGroupStudent> examGroupStudentRepo,
            IRepository<ExamStudentAnswer> examStudentAnswerRepo, 
            IRepository<StudentSectionState> studentSectionstate,
            IStringLocalizer<AnswerSectionRequestHandler> t)
        {
            _sectionRepo = sectionRepo;
            _questionChoiceRepo = questionChoiceRepo;
            _examGroupStudentRepo = examGroupStudentRepo;
            _examStudentAnswerRepo = examStudentAnswerRepo;
            _studentSectionStateRepo = studentSectionstate;
            _t = t;
        }

        public async Task<AnswerActiveSectionDto> Handle(AnswerSectionRequest request, CancellationToken cancellationToken)
        {
            var studentId = request.UserId;
            var section = await GetSectionAsync(request.SectionId, cancellationToken);
            var examGroupStudent = await GetExamGroupStudentAsync(section.ExamId, studentId, cancellationToken);

            CheckQuestions(section.Questions.Select(e => e.Id).ToList(), request.Questions);

            var state = await CorrectSectionAsync(request, section.Questions.ToList(), examGroupStudent, cancellationToken);
            await UpdateStudentSectionState(state, section.Id, studentId, cancellationToken);

            return new() 
            { 
                Id = section.Id,
                State = state
            };
        }

        private void CheckQuestions(List<Guid> questions, List<AnswerQuestion> answerQuestions)
        {
            foreach (var answerQuestion in answerQuestions)
            {
                if (!questions.Contains(answerQuestion.Id))
                    throw new NotFoundException(_t["Question {0} Not Found!", answerQuestion.Id]);
            }
        }

        private async Task<string> CorrectSectionAsync(AnswerSectionRequest request, List<Question> questions, ExamGroupStudent examGroupStudent, CancellationToken cancellationToken)
        {
            List<Question> questionStates = new();
            var answeredQuestions = request.Questions.GroupBy(q => q.Id).Select(group => group.First()).ToList();

            foreach (var question in answeredQuestions)
            {
                var queryQuestion = questions.Find(e => e.Id == question.Id);
                if (!(question.ChoiceId == null && question.StudentAnswer == null))
                {
                    var examStudentAnswer = await _examStudentAnswerRepo.GetBySpecAsync(
                    new ExamStudentAnswerByQuestionIdAndExamGroupStudnetIdSpec(queryQuestion!.Id, examGroupStudent.Id),
                        cancellationToken);

                    if (queryQuestion.Type == QuestionTypes.MCQ)
                    {
                        var queryChoice = await GetQuestionChoiceAsync((Guid)question.ChoiceId!, cancellationToken);
                        double degree = queryChoice.IsRightChoice ? queryQuestion!.Degree : 0;

                        examStudentAnswer!.Update(queryChoice.Id, null, degree, true, true);
                        await _examStudentAnswerRepo.UpdateAsync(examStudentAnswer, cancellationToken);
                    }
                    else
                    {
                        examStudentAnswer!.Update(null, question.StudentAnswer, 0, null, true);
                        await _examStudentAnswerRepo.UpdateAsync(examStudentAnswer, cancellationToken);
                    }

                    
                    questionStates.Add(queryQuestion);
                }
            }

            return questionStates.Count == questions.Count
                ? ESectionState.Reviewing.ToString()
                : questionStates.Count > 0
                    ? ESectionState.InProgress.ToString()
                    : ESectionState.Exploring.ToString();
        }

        private async Task UpdateStudentSectionState(string state, Guid sectionId, Guid userId, CancellationToken cancellationToken)
        {
            var studentSectionState = await GetStudentSectionStateAsync(userId, sectionId, cancellationToken);
            studentSectionState.Update(state);
            await _studentSectionStateRepo.UpdateAsync(studentSectionState, cancellationToken);
        }

        #region DataBase:

        private async Task<StudentSectionState> GetStudentSectionStateAsync(Guid userId, Guid sectionId, CancellationToken cancellationToken)
        {
            var studentSectionState = await _studentSectionStateRepo.GetBySpecAsync(new StudentSectionStateBySectionIdAndUserIdSpec(sectionId, userId), cancellationToken);
            return studentSectionState!;
        }

        private async Task<ExamGroupStudent> GetExamGroupStudentAsync(Guid examId, Guid studentId, CancellationToken cancellationToken)
        {
            var examGroupStudent = await _examGroupStudentRepo.GetBySpecAsync(new ExamGroupStudentIdByExamIdAndStudentIdSpec(examId, studentId), cancellationToken);
            _ = examGroupStudent ?? throw new InternalServerException(_t["ExamGroupStudent Not Found!"]);
            return examGroupStudent;
        }

        private async Task<Section> GetSectionAsync(Guid sectionId, CancellationToken cancellationToken)
        {
            var section = await _sectionRepo.GetBySpecAsync(new SectionsIncludeQuestionsBySectionIdSpec(sectionId), cancellationToken);
            _ = section ?? throw new NotFoundException(_t["Question {0} not found", sectionId]);
            return section;
        }

        private async Task<QuestionChoice> GetQuestionChoiceAsync(Guid questionChoiceId, CancellationToken cancellationToken)
        {
            var questionChoice = await _questionChoiceRepo.GetByIdAsync(questionChoiceId, cancellationToken);
            _ = questionChoice ?? throw new NotFoundException(_t["QuestionChoice {0} not found", questionChoiceId]);
            return questionChoice;
        }

        #endregion
    }



}
