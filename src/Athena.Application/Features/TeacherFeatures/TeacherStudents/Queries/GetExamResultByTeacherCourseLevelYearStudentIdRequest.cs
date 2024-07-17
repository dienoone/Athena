using Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec;

namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Queries
{
    public record GetExamResultByTeacherCourseLevelYearStudentIdRequest(Guid TeacherCourseLevelYearStudentId) : IRequest<List<ExamResultsDto>>;

    public class GetExamResultByTeacherCourseLevelYearStudentIdRequestHandler : IRequestHandler<GetExamResultByTeacherCourseLevelYearStudentIdRequest, List<ExamResultsDto>>
    {
        private readonly IReadRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IReadRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;
        private readonly IStringLocalizer<GetExamResultByTeacherCourseLevelYearStudentIdRequestHandler> _t;

        public GetExamResultByTeacherCourseLevelYearStudentIdRequestHandler(
            IReadRepository<ExamGroupStudent> examGroupStudentRepo, 
            IReadRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo, 
            IStringLocalizer<GetExamResultByTeacherCourseLevelYearStudentIdRequestHandler> t)
        {
            _examGroupStudentRepo = examGroupStudentRepo;
            _teacherCourseLevelYearStudentRepo = teacherCourseLevelYearStudentRepo;
            _t = t;
        }

        public async Task<List<ExamResultsDto>> Handle(GetExamResultByTeacherCourseLevelYearStudentIdRequest request, CancellationToken cancellationToken)
        {
            var student = await _teacherCourseLevelYearStudentRepo.GetByIdAsync(request.TeacherCourseLevelYearStudentId, cancellationToken);
            _ = student ?? throw new NotFoundException(_t["TeacherCourseLevelYearStudent {0} Not Found!", request.TeacherCourseLevelYearStudentId]);

            var examGroupStudents = await _examGroupStudentRepo.ListAsync(
                new ExamGroupStudentsByTeacherCourseLevelYearStudentIdSpec(request.TeacherCourseLevelYearStudentId), cancellationToken);

            List<ExamResultsDto> examResults = new();

            foreach (var examGroupStudent in examGroupStudents)
            {
                var examResultDto =  CreateExamResultDto(examGroupStudent);
                examResults.Add(examResultDto);
            }

            return examResults;
        }

        private static int CalculateExamPoints(double percentage, bool attendance)
        {
            if(attendance)
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

        private static ExamResultsDto CreateExamResultDto(ExamGroupStudent examGroupStudent)
        {
            double percentage = (examGroupStudent.Degree / examGroupStudent.ExamGroup.Exam.FinalDegree) * 100;
            int points = CalculateExamPoints(percentage, examGroupStudent.State);

            return new ExamResultsDto
            {
                ExamId = examGroupStudent.ExamGroup.ExamId,
                ExamName = examGroupStudent.ExamGroup.Exam.Name,
                CreatedOn = examGroupStudent.ExamGroup.Exam.CreatedOn,
                Attendance = examGroupStudent.State,
                ExamDegree = examGroupStudent.ExamGroup.Exam.FinalDegree,
                StudentDegree = examGroupStudent.Degree,
                Percentage = percentage,
                State = examGroupStudent.ExamDegreeState,
                Points = points
            };
        }

    }
}
