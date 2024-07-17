using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;

namespace Athena.Application.Features.TeacherFeatures.Exams.Queries
{
    // ToDo: Add check for state
    public record GetExamResultsByIdRequest(Guid Id) : IRequest<ExamResultDto>;
    public class GetExamResultsByIdRequestValidator : CustomValidator<GetExamResultsByIdRequest>
    {
        public GetExamResultsByIdRequestValidator(IReadRepository<Exam> examRepo, IStringLocalizer<GetExamResultsByIdRequestValidator> T)
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, ct) => await examRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => T["Exam {0} Not Found!", id]);
        }
    }

    public class GetExamResultsByIdRequestHandler : IRequestHandler<GetExamResultsByIdRequest, ExamResultDto>
    {
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IStringLocalizer<GetExamResultsByIdRequestHandler> _t;

        public GetExamResultsByIdRequestHandler(
            IReadRepository<Exam> examRepo, 
            IReadRepository<ExamGroupStudent> examGroupStudentRepo, 
            IStringLocalizer<GetExamResultsByIdRequestHandler> t)
        {
            _examRepo = examRepo;
            _examGroupStudentRepo = examGroupStudentRepo;
            _t = t;
        }

        public async Task<ExamResultDto> Handle(GetExamResultsByIdRequest request, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetBySpecAsync(new ExamResultIncludeExamTypeAndGroupByExamIdSpec(request.Id), cancellationToken);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", request.Id]);

            // Check exam state

            ExamResultDto dto = new()
            {
                Id = exam.Id,
                Name = exam.Name,
                State = exam.State,
                Type = exam.ExamType.Name // Need Include ExamType
            };

            if (exam.ExamGroups.Any())
            {
                var groupDto = await GetGroupDtosAsync(exam.ExamGroups, exam, cancellationToken);
                dto.Groups = groupDto;
            }
           
            return dto;
        }
        private async Task<List<ExamResultGroupDto>> GetGroupDtosAsync(IEnumerable<ExamGroup> examGroups, Exam exam, CancellationToken cancellationToken)
        {
            var groupDtos = new List<ExamResultGroupDto>();

            foreach (var examGroup in examGroups)
            {
                var groupDto = new ExamResultGroupDto
                {
                    Name = examGroup.Group.Name
                };

                var examGroupStudents = await _examGroupStudentRepo.ListAsync(new ExamGroupStudentsByExamGroupIdSpec(examGroup.Id), cancellationToken);

                if (examGroupStudents.Any())
                {
                    var studentDtos = GetStudentDtosAsync(examGroupStudents, exam, examGroup);
                    groupDto.Students = studentDtos;
                }

                groupDtos.Add(groupDto);
            }

            return groupDtos;
        }

        private static List<ExamResultGroupStudentDto> GetStudentDtosAsync(IEnumerable<ExamGroupStudent> examGroupStudents, Exam exam, ExamGroup examGroup)
        {
            var studentDtos = new List<ExamResultGroupStudentDto>();

            foreach (var examGroupStudent in examGroupStudents)
            {
                var studentDto = CreateStudentDto(examGroupStudent, exam, examGroup);
                studentDtos.Add(studentDto);
            }

            return studentDtos;
        }

        private static ExamResultGroupStudentDto CreateStudentDto(ExamGroupStudent examGroupStudent, Exam exam, ExamGroup examGroup)
        {
            var studentDto = new ExamResultGroupStudentDto
            {
                Id = examGroupStudent.Id,
                Image = examGroupStudent.GroupStudent.TeacherCourseLevelYearStudent.Student.Image,
                Name = examGroupStudent.GroupStudent.TeacherCourseLevelYearStudent.Student.Name,
                GroupName = examGroup.Group.Name,
                FinalDegree = (int)exam.FinalDegree,
                StudentDegree = examGroupStudent.Degree,
                State = examGroupStudent.ExamDegreeState,
            };
            studentDto.Percentage = (int)((studentDto.StudentDegree / studentDto.FinalDegree) * 100);

            return studentDto;
        }
    }

}
