using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Update
{
    public record CorrectExamQuestionRequest(Guid ExamGroupStudentId, Guid QuestionId, double Degree) : IRequest<Guid>;

    public class CorrectExamQuestionRequestValidator : CustomValidator<CorrectExamQuestionRequest>
    {
        public CorrectExamQuestionRequestValidator(IReadRepository<ExamGroupStudent> examGroupStudentRepo, IReadRepository<Question> questionRepo,
            IStringLocalizer<CorrectExamQuestionRequestValidator> T)
        {
            RuleFor(e => e.ExamGroupStudentId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (examGroupStudentId, ct) => await examGroupStudentRepo.GetByIdAsync(examGroupStudentId, ct) is not null)
                .WithMessage((_, examGroupStudentId) => T["ExamGroupStudent {0} Not Found!", examGroupStudentId]);

            RuleFor(e => e.QuestionId)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (questionId, ct) => await questionRepo.GetByIdAsync(questionId, ct) is not Question question || question.Type == QuestionTypes.Written)
                .WithMessage((_, questionId) => T["Question {0} not found or does not have the Written type.", questionId]);
        }
    }

    public class CorrectExamQuestionRequestHandler : IRequestHandler<CorrectExamQuestionRequest, Guid>
    {
        private readonly IRepository<ExamStudentAnswer> _examStudentAnswerRepo;
        private readonly IRepository<ExamGroupStudent> _examGroupStudentRepo;

        public CorrectExamQuestionRequestHandler(
            IRepository<ExamStudentAnswer> examStudentAnswerRepo,
            IRepository<ExamGroupStudent> examGroupStudentRepo)
        {
            _examStudentAnswerRepo = examStudentAnswerRepo;
            _examGroupStudentRepo = examGroupStudentRepo;
        }

        public async Task<Guid> Handle(CorrectExamQuestionRequest request, CancellationToken cancellationToken)
        {
            var examGroupStudent = await _examGroupStudentRepo.GetBySpecAsync(new ExamGroupStudentIncludeExamByExamGroupStudentIdSpec(request.ExamGroupStudentId), cancellationToken);

            if (examGroupStudent!.State)
            {
                var examStudentAnswer = await _examStudentAnswerRepo.GetBySpecAsync(
                new ExamStudentAnswerByExamGroupStudentIdAndQuestionIdSpec(request.ExamGroupStudentId, request.QuestionId), cancellationToken);

                // InAnswered Conditions:

                examStudentAnswer!.Update(null, null, request.Degree, true, null);
                await _examStudentAnswerRepo.UpdateAsync(examStudentAnswer, cancellationToken);

                var degree = examGroupStudent.ExamStudentAnswers.Sum(e => e.Degree);
                var percentage = (degree / examGroupStudent.ExamGroup.Exam.FinalDegree) * 100;
                var state = GetStudentState(percentage);
                var points = CalculateExamPoints(percentage, examGroupStudent!.State);

                examGroupStudent!.Update(state, null, degree, points);
                await _examGroupStudentRepo.UpdateAsync(examGroupStudent, cancellationToken);
            }

            return examGroupStudent.Id;
        }

        private static string GetStudentState(double percentage)
        {
            if (percentage >= 95)
                return ExamStudentState.Excellent;
            else if (percentage >= 50 && percentage < 95)
                return ExamStudentState.Successful;
            else
                return ExamStudentState.Failure;
        }

        private static int CalculateExamPoints(double percentage, bool attendance)
        {
            if (attendance)
            {
                if (percentage > 95)
                {
                    return 10;
                }
                else if (percentage >= 50)
                {
                    return 7;
                }
                else
                {
                    return -10;
                }
            }
            else
            {
                return -12;
            }

        }
    }
}
