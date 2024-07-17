using Athena.Application.Features.StudentFeatures.Teachers.Spec;
using Athena.Application.Features.TeacherFeatures.Exams.Commands.Create;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public class CreateExamDependancies : ICreateExamDependancies
    {
        private readonly IRepository<ExamGroup> _examGroupRepo;
        private readonly IRepository<Section> _sectionRepo;
        private readonly IRepository<SectionImage> _sectionImageRepo;
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<QuestionImage> _questionImageRepo;
        private readonly IRepository<QuestionChoice> _questionChoiceRepo;
        private readonly IReadRepository<GroupStudent> _groupStudentRepo;
        private readonly IRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IRepository<ExamStudentAnswer> _examStudentAnswerRepo;
        private readonly IRepository<StudentSectionState> _studentSectionStateRepo;
        private readonly IFileStorageService _file;

        public CreateExamDependancies(
            IRepository<ExamGroup> examGroupRepo, 
            IRepository<Section> sectionRepo, 
            IRepository<SectionImage> sectionImageRepo, 
            IRepository<Question> questionRepo, 
            IRepository<QuestionImage> questionImageRepo, 
            IRepository<QuestionChoice> questionChoiceRepo, 
            IReadRepository<GroupStudent> groupStudentRepo, 
            IRepository<ExamGroupStudent> examGroupStudentRepo, 
            IRepository<ExamStudentAnswer> examStudentAnswerRepo, 
            IRepository<StudentSectionState> studentSectionStateRepo, 
            IFileStorageService file)
        {
            _examGroupRepo = examGroupRepo;
            _sectionRepo = sectionRepo;
            _sectionImageRepo = sectionImageRepo;
            _questionRepo = questionRepo;
            _questionImageRepo = questionImageRepo;
            _questionChoiceRepo = questionChoiceRepo;
            _groupStudentRepo = groupStudentRepo;
            _examGroupStudentRepo = examGroupStudentRepo;
            _examStudentAnswerRepo = examStudentAnswerRepo;
            _studentSectionStateRepo = studentSectionStateRepo;
            _file = file;
        }

        // return examGroupStudentIds, studentIds:
        public async Task<CreateExamHandlerHelper> CreateExamDependanciesAsync(CreateExamRequest request, Guid examId, Guid businessId, CancellationToken cancellationToken)
        {
            List<Guid> studentIds = new();
            List<Guid> examGroupStudentIds = new();

            foreach (Guid id in request.GroupIds)
            {
                ExamGroup examGroup = new(id, examId, false, false, businessId);
                await _examGroupRepo.AddAsync(examGroup, cancellationToken);

                var groupStudents = await _groupStudentRepo.ListAsync(new GroupStudentsByGroupIdSpec(id), cancellationToken);
                foreach (var student in groupStudents)
                {
                    ExamGroupStudent examGroupStudent = new(examGroup.Id, student.Id, ExamStudentState.Absent, false, 0, 0, businessId);
                    await _examGroupStudentRepo.AddAsync(examGroupStudent, cancellationToken);
                    examGroupStudentIds.Add(examGroupStudent.Id);
                    studentIds.Add(student.TeacherCourseLevelYearStudent.StudentId);
                }
            }

            return new()
            {
                ExamGroupStudentIds = examGroupStudentIds,
                StudentIds = studentIds
            };
        }

        public async Task<List<Guid>> CreateSectionsAsync(CreateExamRequest request, List<Guid> examGroupStudentIds, Guid examId, Guid businessId, CancellationToken cancellationToken)
        {
            List<Guid> ids = new();
            foreach (CreateExamRequestSectionHelper section in request.Sections)
            {
                double sectionDegree = section.Questions.Sum(e => e.Degree);
                Section newSection = new(section.Index, section.Name, section.Paragraph, sectionDegree, section.IsPrime, section.Time, examId, businessId);
                await _sectionRepo.AddAsync(newSection, cancellationToken);
                ids.Add(newSection.Id);

                if (section.Images?.Count > 0)
                {
                    foreach (var image in section.Images)
                    {
                        string imagePaht = await _file.UploadAsync<SectionImage>(image.Image, FileType.Image, cancellationToken);
                        SectionImage newSectionImage = new(imagePaht, image.Index, newSection.Id, businessId);
                        await _sectionImageRepo.AddAsync(newSectionImage, cancellationToken);

                    }
                }
                await CreateQuestions(section, newSection.Id, examGroupStudentIds, businessId, cancellationToken);
            }
            return ids;
        }

        private async Task CreateQuestions(CreateExamRequestSectionHelper section, Guid sectionId, List<Guid> examGroupStudnetIds, Guid businessId, CancellationToken cancellationToken)
        {
            foreach (CreateExamRequestQuestionHelper question in section.Questions)
            {
                Question newQuestion = new(question.Index, question.Name, question.Type, question.Answer, question.Degree, question.IsPrime, sectionId, businessId);
                await _questionRepo.AddAsync(newQuestion, cancellationToken);

                if (question.Images?.Count > 0)
                {
                    foreach (var image in question.Images)
                    {
                        string imagePaht = await _file.UploadAsync<QuestionImage>(image.Image, FileType.Image, cancellationToken);
                        QuestionImage newQuestionImage = new(imagePaht, image.Index, newQuestion.Id, businessId);
                        await _questionImageRepo.AddAsync(newQuestionImage, cancellationToken);
                    }
                }

                if (question.Type == QuestionTypes.MCQ)
                {
                    await CreateQuestionChoices(businessId, question, newQuestion, cancellationToken);
                }

                foreach (var examGroupStudentId in examGroupStudnetIds)
                {
                    ExamStudentAnswer examStudentAnswer = new(examGroupStudentId, newQuestion.Id, null, null, 0, false, false, businessId);
                    await _examStudentAnswerRepo.AddAsync(examStudentAnswer, cancellationToken);
                }
            }
        }

        private async Task CreateQuestionChoices(Guid businessId, CreateExamRequestQuestionHelper question, Question newQuestion, CancellationToken cancellationToken)
        {
            foreach (CreateExamRequestQuestionChoicesHelper questionChoice in question.Choices!)
            {
                string image = string.Empty;
                if (questionChoice.Image != null)
                {
                    image = await _file.UploadAsync<QuestionChoice>(questionChoice.Image, FileType.Image, cancellationToken);
                }

                QuestionChoice newQuestionChoice = new(questionChoice.Index, questionChoice.Name, image, questionChoice.IsRightChoice, newQuestion.Id, businessId);
                await _questionChoiceRepo.AddAsync(newQuestionChoice, cancellationToken);
            }
        }

        public async Task CreateStudentExamStatesAsync(List<Guid> sectionIds, List<Guid> studentIds, CancellationToken cancellationToken)
        {
            foreach (var sectionId in sectionIds)
            {
                foreach (var studentId in studentIds)
                {
                    StudentSectionState studentSectionState = new(studentId, sectionId, ESectionState.Exploring.ToString());
                    await _studentSectionStateRepo.AddAsync(studentSectionState, cancellationToken);
                }
            }
        }

        
    }
}
