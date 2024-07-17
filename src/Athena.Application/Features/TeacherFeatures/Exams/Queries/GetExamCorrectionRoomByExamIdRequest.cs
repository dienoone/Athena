using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;
using System.ComponentModel;

namespace Athena.Application.Features.TeacherFeatures.Exams.Queries
{
    public record GetExamCorrectionRoomByExamIdRequest(Guid Id) : IRequest<ExamCorrectionRoomDto>;

    public class GetExamCorrectionRoomByExamIdRequestHandler : IRequestHandler<GetExamCorrectionRoomByExamIdRequest, ExamCorrectionRoomDto>
    {
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IReadRepository<ExamStudentAnswer> _examStudentAnswerRepo;
        private readonly IStringLocalizer<GetExamCorrectionRoomByExamIdRequestHandler> _t;

        public GetExamCorrectionRoomByExamIdRequestHandler(
            IReadRepository<Exam> examRepo,
            IReadRepository<ExamGroupStudent> examGroupStudentRepo,
            IReadRepository<ExamStudentAnswer> examStudentAnswerRepo,
            IStringLocalizer<GetExamCorrectionRoomByExamIdRequestHandler> t)
        {
            _examRepo = examRepo;
            _examGroupStudentRepo = examGroupStudentRepo;
            _examStudentAnswerRepo = examStudentAnswerRepo;
            _t = t;
        }
        public async Task<ExamCorrectionRoomDto> Handle(GetExamCorrectionRoomByExamIdRequest request, CancellationToken cancellationToken)
        {
            var exam = await GetExamAsync(request.Id, cancellationToken);
            var examStudentAnswers = await _examStudentAnswerRepo.ListAsync(new ExamStudentAnswersByExamIdSpec(exam.Id), cancellationToken);

            var dto = new ExamCorrectionRoomDto
            {
                ExamId = request.Id,
                Name = exam.Name,
                StartCorrect = examStudentAnswers.Any(e => e.IsCorrected),
            };

            bool result = true;
            foreach(var answer in examStudentAnswers)
            {
                if (answer.IsAnswered)
                {
                    if (!answer.IsCorrected)
                    {
                        result = false;
                        break;
                    }
                }
            }
            dto.IsFinished = result;

            var groupDtos = await GetGroupDtosAsync(exam, cancellationToken);
            dto.Groups = groupDtos;

            return dto;
        }

        private async Task<Exam> GetExamAsync(Guid examId, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetBySpecAsync(new ExamIncludeExamGroupWithGroupByExamIdSpec(examId), cancellationToken);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", examId]);

            if (exam.State == ExamState.Active && exam.State == ExamState.Upcoming)
                throw new ConflictException(_t["Exam state is {0}, you can't access this now", exam.State]);

            return exam;
        }

        private async Task<List<ExamCorrectionRoomGoupDto>> GetGroupDtosAsync(Exam exam, CancellationToken cancellationToken)
        {
            var groupDtos = new List<ExamCorrectionRoomGoupDto>();

            foreach (var group in exam.ExamGroups)
            {
                var groupDto = new ExamCorrectionRoomGoupDto
                {
                    Name = group.Group.Name
                };

                var examGroupStudents = await GetExamGroupStudentsAsync(group.Id, cancellationToken);

                if (examGroupStudents.Any())
                {
                    var studentDtos = GetStudentDtos(examGroupStudents, exam, group);
                    groupDto.Students = studentDtos;
                }

                groupDtos.Add(groupDto);
            }

            return groupDtos;
        }

        private async Task<List<ExamGroupStudent>> GetExamGroupStudentsAsync(Guid groupId, CancellationToken cancellationToken)
        {
            return await _examGroupStudentRepo.ListAsync(new ExamGroupStudentsIncludeStudentByExamGroupIdSpec(groupId), cancellationToken);
        }

        private static List<ExamCorrectionRoomStudentDto> GetStudentDtos(List<ExamGroupStudent> examGroupStudents, Exam exam, ExamGroup examGroup)
        {
            var studentDtos = new List<ExamCorrectionRoomStudentDto>();

            foreach (var examGroupStudent in examGroupStudents)
            {
                var student = examGroupStudent.GroupStudent.TeacherCourseLevelYearStudent.Student;

                var studentDto = new ExamCorrectionRoomStudentDto
                {
                    Id = examGroupStudent.Id,
                    Image = student.Image,
                    Name = student.Name,
                    GroupName = examGroup.Group.Name,
                    FinalDegree = (int)exam.FinalDegree,
                    StudentDegree = (int)examGroupStudent.Degree,
                    Percentage = (int)((examGroupStudent.Degree / exam.FinalDegree) * 100),
                    State = examGroupStudent.ExamDegreeState
                };

                if (examGroupStudent.State)
                {
                    studentDto.IsFinish = examGroupStudent.ExamStudentAnswers.All(e => e.IsCorrected);
                }
                else
                {
                    studentDto.IsFinish = true;
                }
                
                

                studentDtos.Add(studentDto);
            }

            return studentDtos;
        }
    }
}
