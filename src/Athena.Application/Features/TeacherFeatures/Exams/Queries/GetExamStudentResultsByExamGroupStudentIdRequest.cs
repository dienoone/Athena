using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Queries
{
    public record GetExamStudentResultsByExamGroupStudentIdRequest(Guid Id) : IRequest<StudentAnswerDto>;

    public class GetExamStudentResultsByExamStudentIdRequestHandler : IRequestHandler<GetExamStudentResultsByExamGroupStudentIdRequest, StudentAnswerDto>
    {
        private readonly IReadRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<Section> _sectionRepo;
        private readonly IReadRepository<Question> _questionRepo;
        private readonly IStringLocalizer<GetExamStudentResultsByExamStudentIdRequestHandler> _t;

        public GetExamStudentResultsByExamStudentIdRequestHandler(
            IReadRepository<ExamGroupStudent> examGroupStudentRepo, 
            IReadRepository<Exam> examRepo, 
            IReadRepository<Section> sectionRepo, 
            IReadRepository<Question> questionRepo, 
            IStringLocalizer<GetExamStudentResultsByExamStudentIdRequestHandler> t)
        {
            _examGroupStudentRepo = examGroupStudentRepo;
            _examRepo = examRepo;
            _sectionRepo = sectionRepo;
            _questionRepo = questionRepo;
            _t = t;
        }

        public async Task<StudentAnswerDto> Handle(GetExamStudentResultsByExamGroupStudentIdRequest request, CancellationToken cancellationToken)
        {
            var examGroupStudent = await GetExamGroupStudentAsync(request.Id, cancellationToken);
            var exam = await GetExamWithDetailsAsync(examGroupStudent.ExamGroup.ExamId, cancellationToken);
            var sections = await GetSectionsWithDetailsAsync(exam!.Id, cancellationToken);
            var student = examGroupStudent.GroupStudent.TeacherCourseLevelYearStudent.Student;

            List<ExamStudentAnswerSectionDto> sectionDtos = new();
            foreach (var section in sections)
            {
                var sectionDto = await BuildSectionDtoAsync(section, examGroupStudent, cancellationToken);
                sectionDtos.Add(sectionDto);
            }

            return new()
            {
                Id = exam.Id,
                ExamName = exam.Name,
                StudentName = student.Name,
                Gender = student.Gender,
                StudentDegree = examGroupStudent.Degree,
                Sections = sectionDtos
            };

        }

        #region Getters:

        private async Task<ExamGroupStudent> GetExamGroupStudentAsync(Guid examGroupStudentId, CancellationToken cancellationToken)
        {
            var examGroupStudent = await _examGroupStudentRepo.GetBySpecAsync(new ExamGroupStudentIncludeAnswersByExamGroupStudentIdSpec(examGroupStudentId), cancellationToken);
            _ = examGroupStudent ?? throw new InternalServerException(_t["ExamGroupStudent Not Found!"]);
            return examGroupStudent;
        }
        private async Task<Exam?> GetExamWithDetailsAsync(Guid examId, CancellationToken cancellationToken)
        {
            return await _examRepo.GetBySpecAsync(new ExamByExamIdIncludeExamTypeSpec(examId), cancellationToken);
        }
        private async Task<List<Section>> GetSectionsWithDetailsAsync(Guid examId, CancellationToken cancellationToken)
        {
            return await _sectionRepo.ListAsync(new SectionsDetailByExamIdSpec(examId), cancellationToken);
        }
        private async Task<List<Question>> GetQuestionsForSectionAsync(Guid sectionId, CancellationToken cancellationToken)
        {
            var questions = await _questionRepo.ListAsync(new QuestionsDetailBySectionIdSpec(sectionId), cancellationToken);
            return questions;
        }

        #endregion

        #region Maps:

        private async Task<ExamStudentAnswerSectionDto> BuildSectionDtoAsync(Section section, ExamGroupStudent examGroupStudent, CancellationToken cancellationToken)
        {
            var sectionImages = section.SectionImages?.Select(image => new ImageDetailDto
            {
                Id = image.Id,
                Index = image.Index,
                Image = image.Image

            }).ToList();

            var questions = await GetQuestionsForSectionAsync(section.Id, cancellationToken);

            var questionDtos = new List<ExamStudentAnswerQuestionDto>();
            foreach (var question in questions)
            {
                var questionDto = BuildQuestionDto(question, examGroupStudent);
                questionDtos.Add(questionDto);
            }


            return new ExamStudentAnswerSectionDto
            {
                Id = section.Id,
                Index = section.Index,
                Name = section.Name,
                Paragraph = section.Paragraph,
                Degree = section.Degree,
                IsPrime = section.IsPrime,
                Images = sectionImages,
                Questions = questionDtos
            };
        }

        private static ExamStudentAnswerQuestionDto BuildQuestionDto(Question question, ExamGroupStudent examGroupStudent)
        {
            var questionImages = question.QuestionImages?.Select(image => new ImageDetailDto
            {
                Id = image.Id,
                Index = image.Index,
                Image = image.Image

            }).ToList();

            var questionChoices = question.QuestionChoices?.Select(choice => new QuestionChoiceDetailDto
            {
                Id = choice.Id,
                Index = choice.Index,
                Name = choice.Name,
                Image = choice.Image,
                IsRightChoice = choice.IsRightChoice

            }).ToList();

            var examStudentAnswer = examGroupStudent.ExamStudentAnswers.FirstOrDefault(e => e.QuestionId == question.Id && e.ExamGroupStudentId == examGroupStudent.Id);
            
            return new ExamStudentAnswerQuestionDto
            {
                Id = question.Id,
                Index = question.Index,
                Name = question.Name,
                Type = question.Type,
                Answer = (question.Type == QuestionTypes.Written) ? question.Answer : "",
                Degree = question.Degree,
                IsPrime = question.IsPrime,
                StudentAnswer = (question.Type == QuestionTypes.MCQ) ? examStudentAnswer?.QuestionChoiceId.ToString() : examStudentAnswer?.Answer,
                StudentDegree = CalculateQuestionStudentDegree(question, examStudentAnswer),
                IsCorrected = examStudentAnswer != null && examStudentAnswer.IsCorrected,
                IsAnswered = examStudentAnswer!.IsAnswered,
                Images = questionImages,
                Choices = questionChoices
            };
        }

        private static double CalculateQuestionStudentDegree(Question question, ExamStudentAnswer? examStudentAnswer)
        {
            bool isRightAnswer = false;

            if (question.Type == QuestionTypes.MCQ)
            {
                var choice = question.QuestionChoices!.FirstOrDefault(c => examStudentAnswer?.QuestionChoiceId == c.Id);
                isRightAnswer = choice?.IsRightChoice ?? false;
            }
            else if (question.Type == QuestionTypes.Written)
            {
                isRightAnswer = true; // Written questions are assumed to always have the correct answer.
            }

            return (isRightAnswer ? (question.Type == QuestionTypes.Written ? examStudentAnswer?.Degree ?? 0 : question.Degree) : 0);
        }

        #endregion

    }


}
