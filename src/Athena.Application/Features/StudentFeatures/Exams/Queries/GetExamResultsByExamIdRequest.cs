using Athena.Application.Features.StudentFeatures.Exams.Dtos;
using Athena.Application.Features.StudentFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Exams.Queries
{
    public record GetExamResultsByExamIdRequest(Guid Id) : IRequest<ExamResultDto>;

    public class GetExamResultsByExamIdRequestHandler : IRequestHandler<GetExamResultsByExamIdRequest, ExamResultDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<Section> _sectionRepo;
        private readonly IReadRepository<Question> _questionRepo;
        private readonly IStringLocalizer<GetExamResultsByExamIdRequestHandler> _t;

        public GetExamResultsByExamIdRequestHandler(
            ICurrentUser currentUser,
            IReadRepository<ExamGroupStudent> examGroupStudentRepo,
            IReadRepository<Exam> examRepo,
            IReadRepository<Section> sectionRepo,
            IReadRepository<Question> questionRepo,
            IStringLocalizer<GetExamResultsByExamIdRequestHandler> t, 
            IReadRepository<Teacher> teacherRepo)
        {
            _examGroupStudentRepo = examGroupStudentRepo;
            _examRepo = examRepo;
            _sectionRepo = sectionRepo;
            _questionRepo = questionRepo;
            _currentUser = currentUser;
            _teacherRepo = teacherRepo;
            _t = t;
        }

        public async Task<ExamResultDto> Handle(GetExamResultsByExamIdRequest request, CancellationToken cancellationToken)
        {
            var examGroupStudent = await GetExamGroupStudentAsync(request.Id, cancellationToken);
            var exam = await GetExamWithDetailsAsync(request.Id, cancellationToken);
            var sections = await GetSectionsWithDetailsAsync(exam!.Id, cancellationToken);
            var teacher = await GetTeacherAsync(exam.BusinessId, cancellationToken);

            return await BuildExamResultDtoAsync(exam, teacher!, examGroupStudent,sections, cancellationToken);
        }


        private static ExamResultDegreeDto CalculateExamResultDegree(Exam exam, ExamGroupStudent examGroupStudent, List<ExamResultSectionDto> sections)
        {
            double percentage = (examGroupStudent.Degree / exam.FinalDegree) * 100;

            var examResultDegreeDto = new ExamResultDegreeDto
            {
                ExamDegree = exam.FinalDegree,
                StudentDegree = examGroupStudent.Degree,
                Percentage = percentage,
                Status = examGroupStudent.ExamDegreeState
            };
            return examResultDegreeDto;
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
       
        private async Task<ExamResultDto> BuildExamResultDtoAsync(Exam exam, Teacher teacher, ExamGroupStudent examGroupStudent, List<Section> sections, CancellationToken cancellationToken)
        {
            var sectionDtos = new List<ExamResultSectionDto>();
            foreach (var section in sections)
            {
                var sectionDto = await BuildSectionDtoAsync(section, examGroupStudent, cancellationToken);
                sectionDtos.Add(sectionDto);
            }

            return new ExamResultDto
            {
                Id = exam.Id,
                Name = exam.Name,
                Course = teacher.Course.Name,
                Teacher = teacher.Name,
                TeacherImage = teacher.ImagePath,
                ExamReult = CalculateExamResultDegree(exam, examGroupStudent, sectionDtos),
                Sections = sectionDtos
            };
        }

        private async Task<ExamResultSectionDto> BuildSectionDtoAsync(Section section, ExamGroupStudent examGroupStudent, CancellationToken cancellationToken)
        {
            var sectionImages = section.SectionImages?.Select(image => new ActiveImageDetailDto
            {
                Id = image.Id,
                Index = image.Index,
                Image = image.Image
            }).ToList();

            var questions = await GetQuestionsForSectionAsync(section.Id, cancellationToken);
            
            var questionDtos = new List<ExamResultQuestionDto>();
            foreach(var question in questions)
            {
                var questionDto = BuildQuestionDto(question, examGroupStudent);
                questionDtos.Add(questionDto);
            }
            

            return new ExamResultSectionDto
            {
                Id = section.Id,
                Name = section.Name,
                Paragraph = section.Paragraph,
                Degree = questionDtos.Sum(e => e.Degree),
                IsPrime = section.IsPrime,
                Images = sectionImages,
                Questions = questionDtos
            };
        }

        private static ExamResultQuestionDto BuildQuestionDto(Question question, ExamGroupStudent examGroupStudent)
        {
            var questionImages = question.QuestionImages?.Select(image => new ActiveImageDetailDto
            {
                Id = image.Id,
                Index = image.Index,
                Image = image.Image

            }).ToList();

            var questionChoices = question.QuestionChoices?.Select(choice => new ExamResultChoiceDto
            {
                Id = choice.Id,
                Name = choice.Name,
                Image = choice.Image,
                IsRightChoice = choice.IsRightChoice

            }).ToList();

            var examStudentAnswer = examGroupStudent.ExamStudentAnswers.FirstOrDefault(e => e.QuestionId == question.Id && e.ExamGroupStudentId == examGroupStudent.Id);
;
            return new ExamResultQuestionDto
            {
                Id = question.Id,
                Name = question.Name,
                Type = question.Type,
                Degree = question.Degree,
                IsPrime = question.IsPrime,
                StudentAnswer = (question.Type == QuestionTypes.MCQ) ? examStudentAnswer?.QuestionChoiceId.ToString() : examStudentAnswer?.Answer,
                StudentDegree = CalculateQuestionStudentDegree(question, examStudentAnswer),
                Images = questionImages,
                Choices = questionChoices
            };
        }

        #region Database:

        private async Task<Teacher?> GetTeacherAsync(Guid businessId, CancellationToken cancellationToken)
        {
            return await _teacherRepo.GetBySpecAsync(new TeacherByBusinessIdIncludeCourseSpec(businessId), cancellationToken);
        }

        private async Task<ExamGroupStudent> GetExamGroupStudentAsync(Guid examId, CancellationToken cancellationToken)
        {
            var examGroupStudent = await _examGroupStudentRepo.GetBySpecAsync(new ExamGroupStudentIdByExamIdAndStudentIdIncludeAnswersSpec(examId, _currentUser.GetUserId()), cancellationToken);
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

    }
}
