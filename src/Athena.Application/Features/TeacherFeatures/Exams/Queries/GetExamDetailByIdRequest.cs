using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Queries
{
    public record GetExamDetailByIdRequest(Guid Id) : IRequest<ExamDetailDto>;

    public class GetExamDetailByIdRequestValidator : CustomValidator<GetExamDetailByIdRequest>
    {
        public GetExamDetailByIdRequestValidator(IReadRepository<Exam> examRepo, IStringLocalizer<GetExamDetailByIdRequestValidator> T)
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, ct) => await examRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => T["Exam {0} Not Found.", id]);
        }
    }

    public class GetExamDetailByIdRequestHandler : IRequestHandler<GetExamDetailByIdRequest, ExamDetailDto>
    {
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<ExamGroup> _examGroupRepo;
        private readonly IReadRepository<Section> _sectionRepo;
        private readonly IReadRepository<Question> _questionRepo;

        public GetExamDetailByIdRequestHandler(
            IReadRepository<Exam> examRepo, 
            IReadRepository<ExamGroup> examGroupRepo, 
            IReadRepository<Section> sectionRepo, 
            IReadRepository<Question> questionRepo)
        {
            _examRepo = examRepo;
            _examGroupRepo = examGroupRepo;
            _sectionRepo = sectionRepo;
            _questionRepo = questionRepo;
        }

        public async Task<ExamDetailDto> Handle(GetExamDetailByIdRequest request, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetBySpecAsync(new ExamDetailByIdSpec(request.Id), cancellationToken);
            var sections = await _sectionRepo.ListAsync(new SectionsDetailByExamIdSpec(request.Id), cancellationToken);
            List<SectionDetailDto> sectionDetailDtos= new();

            foreach (var section in sections)
            {
                List<QuestionDetailDto> questionDetailDtos= new();
                var questions = await _questionRepo.ListAsync(new QuestionsDetailBySectionIdSpec(section.Id), cancellationToken);
                foreach (var question in questions)
                {
                    QuestionDetailDto questionDetailDto = MapQuestion(question);
                    questionDetailDtos.Add(questionDetailDto);
                }
                SectionDetailDto sectionDetailDto = MapSection(section); 
                sectionDetailDto.Questions = questionDetailDtos;
                sectionDetailDtos.Add(sectionDetailDto);
            }

            ExamDetailDto dto = MapExam(exam!);
            dto.NumberOfSections = sections.Count;
            dto.Sections= sectionDetailDtos;


            var groups = await _examGroupRepo.ListAsync(new ExamGroupsByExamIdIncludeGroupSpec(request.Id), cancellationToken);
            dto.Groups = groups?.Adapt<List<ExamGroupDto>>();
            
            return dto;
        }

        private static ExamDetailDto MapExam(Exam exam)
        {
            return new()
            {
                Id = exam.Id,
                Name = exam.Name,
                YearStart = exam.TeacherCourseLevelYear.Year.DashboardYear.Start,
                YearEnd = exam.TeacherCourseLevelYear.Year.DashboardYear.Start + 1,
                LevelName = exam.TeacherCourseLevelYear.TeacherCourseLevel.Level.Name,
                PublishedDate = exam.PublishedDate,
                PublishedTime = exam.PublishedTime,
                AllowedTime = exam.AllowedTime,
                FinalDegree = exam.FinalDegree,
                Description = exam.Description,
                State = exam.State,
                IsActive = exam.IsActive,
                IsPrime = exam.IsPrime,
                IsReady = exam.IsReady,
                TeacherCourseLevelYearId = exam.TeacherCourseLevelYearId,
                ExamTypeId = exam.ExamTypeId,
                ExamType = exam.ExamType.Name,
                CreatedOn = exam.CreatedOn,
            };
        }
        private static SectionDetailDto MapSection(Section section)
        {
            List<ImageDetailDto> Images = new();

            if (section.SectionImages != null)
            {
                foreach (var image in section.SectionImages)
                {
                    ImageDetailDto imageDetailDto = new()
                    {
                        Id = image.Id,
                        Index = image.Index,
                        Image = image.Image,
                    };
                    Images.Add(imageDetailDto);
                }
            }

            return new()
            {
                Id = section.Id,
                Index = section.Index,
                Name = section.Name,
                Paragraph = section.Paragraph,
                Degree = section.Degree,
                IsPrime = section.IsPrime,
                Time = section.Time,
                Images = Images.Count > 0 ? Images : null,
            };
        }
        private static QuestionDetailDto MapQuestion(Question question)
        {
            List<ImageDetailDto> Images = new List<ImageDetailDto>();
            List<QuestionChoiceDetailDto> questionChoices= new List<QuestionChoiceDetailDto>();

            if (question.QuestionImages != null)
            {
                foreach (var image in question.QuestionImages)
                {
                    ImageDetailDto imageDetailDto = new()
                    {
                        Id = image.Id,
                        Index = image.Index,
                        Image = image.Image,
                    };
                    Images.Add(imageDetailDto);
                }
            }

            if(question.Type == QuestionTypes.MCQ)
            {
                foreach (var choice in question.QuestionChoices!)
                {
                    QuestionChoiceDetailDto questionChoice = new()
                    {
                        Id= choice.Id,
                        Index = choice.Index,
                        Name= choice.Name,
                        Image= choice.Image,
                        IsRightChoice= choice.IsRightChoice
                    };
                    questionChoices.Add(questionChoice);
                }
            }

            return new() 
            {
                Id= question.Id,
                Index= question.Index,
                Name= question.Name,
                Type= question.Type,
                Answer= question.Answer,
                Degree= question.Degree,
                IsPrime= question.IsPrime,
                Images = Images.Count > 0 ? Images : null,
                Choices= questionChoices.Count > 0 ? questionChoices : null
            };
        }
    }



}
