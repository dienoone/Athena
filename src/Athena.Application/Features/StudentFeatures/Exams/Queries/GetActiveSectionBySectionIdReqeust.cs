using Athena.Application.Features.StudentFeatures.Exams.Dtos;
using Athena.Application.Features.StudentFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Exams.Queries
{
    public record GetActiveSectionBySectionIdReqeust(Guid Id, Guid UserId) : IRequest<ActiveSectionDto>;

    public class GetActiveSectionBySectionIdReqeustHandler : IRequestHandler<GetActiveSectionBySectionIdReqeust, ActiveSectionDto>
    {
        private readonly IReadRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IReadRepository<Section> _sectionRepo;
        private readonly IReadRepository<Question> _questionRepo;
        private readonly IStringLocalizer<GetExamResultsByExamIdRequestHandler> _t;

        public GetActiveSectionBySectionIdReqeustHandler(
            IReadRepository<ExamGroupStudent> examGroupStudentRepo,
            IReadRepository<Section> sectionRepo,
            IReadRepository<Question> questionRepo, 
            IStringLocalizer<GetExamResultsByExamIdRequestHandler> t)
        {
            _examGroupStudentRepo = examGroupStudentRepo;
            _sectionRepo = sectionRepo;
            _questionRepo = questionRepo;
            _t = t;
        }

        public async Task<ActiveSectionDto> Handle(GetActiveSectionBySectionIdReqeust request, CancellationToken cancellationToken)
        {
            var section = await GetSectionAsync(request.Id, cancellationToken);
            var examGroupStudent = await GetExamGroupStudentAsync(section.ExamId, request.UserId, cancellationToken);
            var questionDtos = await GetQuestionDtos(section.Id, examGroupStudent, cancellationToken);

            return BuildSectionDto(section, questionDtos);
        }

        private async Task<ExamGroupStudent> GetExamGroupStudentAsync(Guid examId, Guid userId, CancellationToken cancellationToken)
        {
            var examGroupStudent = await _examGroupStudentRepo.GetBySpecAsync(new ExamGroupStudentIdByExamIdAndStudentIdIncludeAnswersSpec(examId, userId), cancellationToken);
            _ = examGroupStudent ?? throw new InternalServerException(_t["ExamGroupStudent Not Found!"]);
            return examGroupStudent;
        }

        private async Task<Section> GetSectionAsync(Guid sectionId, CancellationToken cancellationToken)
        {
            var section = await _sectionRepo.GetBySpecAsync(new SectionDetailBySectioinIdSpec(sectionId), cancellationToken);
            _ = section ?? throw new NotFoundException(_t["Section {0} Not Found!", sectionId]);
            return section;
        }

        private static ActiveSectionDto BuildSectionDto(Section section, List<ActiveQuestionDto> questionDtos)
        {
            var sectionImages = section.SectionImages?.Select(image => new ActiveImageDetailDto
            {
                Id = image.Id,
                Index = image.Index,
                Image = image.Image

            }).ToList();

            return new ActiveSectionDto
            {
                Id = section.Id,
                Name = section.Name,
                Paragraph = section.Paragraph,
                Images = sectionImages,
                Questions = questionDtos
            };
        }

        private async Task<List<ActiveQuestionDto>> GetQuestionDtos(Guid sectionId, ExamGroupStudent examGroupStudent, CancellationToken cancellationToken)
        {
            List<ActiveQuestionDto> questionDtos = new();
            var questions = await _questionRepo.ListAsync(new QuestionsDetailBySectionIdSpec(sectionId), cancellationToken);
            foreach (var question in questions)
            {
                questionDtos.Add(BuildQuestionDto(question, examGroupStudent));
            }
            return questionDtos;
        }

        private static ActiveQuestionDto BuildQuestionDto(Question question, ExamGroupStudent examGroupStudent)
        {
            var questionImages = question.QuestionImages?.Select(image => new ActiveImageDetailDto
            {
                Id = image.Id,
                Index = image.Index,
                Image = image.Image

            }).ToList();

            var questionChoices = question.QuestionChoices?.Select(choice => new ActiveQuestionChoiceDto
            {
                Id = choice.Id,
                Name = choice.Name,
                Image = choice.Image,

            }).ToList();

            var examStudentAnswer = examGroupStudent.ExamStudentAnswers.FirstOrDefault(e => e.QuestionId == question.Id && e.ExamGroupStudentId == examGroupStudent.Id);
            ;
            return new ActiveQuestionDto
            {
                Id = question.Id,
                Name = question.Name,
                Type = question.Type,
                Degree = question.Degree,
                IsPrime = question.IsPrime,
                StudentAnswer = (question.Type == QuestionTypes.MCQ) ? examStudentAnswer?.QuestionChoiceId.ToString() : examStudentAnswer?.Answer,
                Images = questionImages,
                Choices = questionChoices
            };
        }
    }
}
