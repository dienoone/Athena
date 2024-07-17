using Athena.Application.Features.TeacherFeatures.Home.Dtos;
using Athena.Application.Features.TeacherFeatures.Home.Spec;
using Athena.Domain.Common.Const;
using System.Globalization;

namespace Athena.Application.Features.TeacherFeatures.Home.Queries
{
    public record GetTeacherHomeRequest() : IRequest<TeacherHomeRequestDto>;

    public class GetHomeRequestHandler : IRequestHandler<GetTeacherHomeRequest, TeacherHomeRequestDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;
        private readonly IReadRepository<Group> _groupRepo;
        private readonly IReadRepository<SignalRConnectionGroup> _signalRConnectionGroupRepo;
        private readonly IReadRepository<GroupScadual> _groupScadualRepo;
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<ExamGroupStudent> _examStudentGroupRepo;

        public GetHomeRequestHandler(
            ICurrentUser currentUser, 
            IReadRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo, 
            IReadRepository<Group> groupRepo, 
            IReadRepository<SignalRConnectionGroup> signalRConnectionGroupRepo, 
            IReadRepository<GroupScadual> groupScadualRepo, 
            IReadRepository<Exam> examRepo, 
            IReadRepository<ExamGroupStudent> examStudentGroupRepo)
        {
            _currentUser = currentUser;
            _teacherCourseLevelYearStudentRepo = teacherCourseLevelYearStudentRepo;
            _groupRepo = groupRepo;
            _signalRConnectionGroupRepo = signalRConnectionGroupRepo;
            _groupScadualRepo = groupScadualRepo;
            _examRepo = examRepo;
            _examStudentGroupRepo = examStudentGroupRepo;
        }

        public async Task<TeacherHomeRequestDto> Handle(GetTeacherHomeRequest request, CancellationToken cancellationToken)
        {
            Guid businessId = _currentUser.GetBusinessId();

            var lastResultDto = await MapLastResultDto(businessId, cancellationToken);
            var upcommingLessonsDtos = await UpcommingLessonsDtos(businessId, cancellationToken);

            var exams = await GetExamsAsync(businessId, cancellationToken);

            var teacherHomeExamDto = MapExamsDtosAsync(exams);
            var teacherHomeExamReportDto = await MapTeacherHomeExamReportDto(exams.Where(e => e.State == ExamState.Finished), cancellationToken);


            return new()
            {
                LastResult = lastResultDto,
                UpcommingLessons = upcommingLessonsDtos,
                Exams = teacherHomeExamDto,
                ExamReport = teacherHomeExamReportDto
            };
        }


        #region Mappers:
        
        private async Task<LastResultDto> MapLastResultDto(Guid businessId, CancellationToken cancellationToken)
        {
            var students = await GetTeacherCourseLevelYearStudents(businessId, cancellationToken);
            var activeStudents = await GetConnectionGroupsAsync($"GroupTeacher-{businessId}", cancellationToken);
            var groups = await GetGroupsAsync(businessId, cancellationToken);

            return new() 
            { 
                AssignedStudents = students.Count(),
                ActiveStudents = activeStudents.Count(),
                Groups = groups.Count(),
            };
        }

        private async Task<List<UpcommingLessonsDto>> UpcommingLessonsDtos(Guid businessId, CancellationToken cancellationToken)
        {
            List<UpcommingLessonsDto> dtos = new();

            var groupScaduals = await GetGroupScadualsAsync(DateTime.Today.ToString("dddd", CultureInfo.InvariantCulture), businessId, cancellationToken);

            if (groupScaduals.Any())
            {
                foreach (var scadual in groupScaduals)
                {
                    UpcommingLessonsDto dto = new() 
                    { 
                        GroupName = scadual.Group.Name,
                        HeadQuarterName = scadual.Group.HeadQuarter.Name,
                        Time = scadual.StartTime
                    };
                    dtos.Add(dto);
                }
            }

            return dtos;
        }

        private TeacherHomeExamDto MapExamsDtosAsync(IEnumerable<Exam> exams)
        {
            TeacherHomeExamDto resultDto = new();

            foreach (var exam in exams)
            {
                Console.WriteLine($"Id: {exam.Id}, Name: {exam.Name}");
            }

            if (exams.Any())
            {
                resultDto.AssignedExams = exams.Count();
                resultDto.ActiveExams = exams.Count(e => e.State == ExamState.Active);
                resultDto.CorrectingExams = exams.Count(e => e.State == ExamState.Correcting);
            }

            return resultDto;
        }

        private async Task<TeacherHomeExamReportDto?> MapTeacherHomeExamReportDto(IEnumerable<Exam> exams, CancellationToken cancellationToken)
        {
            if (exams.Any())
            {
                TeacherHomeExamReportDto resultDto = new();

                resultDto.January = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 1), cancellationToken);
                resultDto.February = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 2), cancellationToken);
                resultDto.March = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 3), cancellationToken);
                resultDto.April = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 4), cancellationToken);
                resultDto.May = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 5), cancellationToken);
                resultDto.June = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 6), cancellationToken);
                resultDto.July = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 7), cancellationToken);
                resultDto.August = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 8), cancellationToken);
                resultDto.September = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 9), cancellationToken);
                resultDto.October = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 10), cancellationToken);
                resultDto.November = await HelperTeacherHomeExamReportDto(exams.Where(e =>  e.PublishedDate.Month == 11), cancellationToken);
                resultDto.December = await HelperTeacherHomeExamReportDto(exams.Where(e => e.PublishedDate.Month == 12), cancellationToken);

                return resultDto;
            }
            return null;
        }

        #region Helpers:

        private async Task<MonthExamReportDto?> HelperTeacherHomeExamReportDto(IEnumerable<Exam> exams, CancellationToken cancellationToken)
        {
            if (exams.Any())
            {
                IEnumerable<ExamGroupStudent> allExamGroupStudents = await GetExamGroupStudentsAsync(exams.Select(e => e.Id), cancellationToken);
                if (allExamGroupStudents.Any())
                {
                    return MapMonthExamReportDto(allExamGroupStudents);
                }
            }
            return null;
        }

        private static MonthExamReportDto? MapMonthExamReportDto(IEnumerable<ExamGroupStudent> students)
        {
            if (students.Any())
            {
                MonthExamReportDto monthExamReportDto = new()
                {
                    Distinctive = students.Count(e => e.ExamDegreeState == ExamStudentState.Excellent),
                    Successed = students.Count(e => e.ExamDegreeState == ExamStudentState.Successful),
                    Failed = students.Count(e => e.ExamDegreeState == ExamStudentState.Failure),
                    Absent = students.Count(e => e.ExamDegreeState == ExamStudentState.Absent),
                };
                return monthExamReportDto;
            }

            return null;
        }

        #endregion


        #endregion

        #region Database:

        private async Task<IEnumerable<TeacherCourseLevelYearStudent>> GetTeacherCourseLevelYearStudents(Guid businessId, CancellationToken cancellationToken)
        {
            return await _teacherCourseLevelYearStudentRepo.ListAsync(new TeacherCourseLevelYearStudentsByBusinessIdSpec(businessId), cancellationToken);
        }
        private async Task<IEnumerable<Group>> GetGroupsAsync(Guid businessId, CancellationToken cancellationToken)
        {
            return await _groupRepo.ListAsync(new GroupsByBusinessIdSpec(businessId), cancellationToken);
        }
        private async Task<IEnumerable<SignalRConnectionGroup>> GetConnectionGroupsAsync(string groupName, CancellationToken cancellationToken)
        {
            return await _signalRConnectionGroupRepo.ListAsync(new SignalRConnectionGroupByGroupNameSpec(groupName), cancellationToken);
        }
        private async Task<IEnumerable<GroupScadual>> GetGroupScadualsAsync(string day, Guid businessId, CancellationToken cancellationToken)
        {
            return await _groupScadualRepo.ListAsync(new GroupScadualsIncludeGroupByDayAndBusinessIdSpec(day, businessId), cancellationToken);
        }
        private async Task<IEnumerable<Exam>> GetExamsAsync(Guid businssId, CancellationToken cancellationToken)
        {
            return await _examRepo.ListAsync(new ExamsByOpenYearAndBusniessIdSpec(businssId), cancellationToken);
        }
        private async Task<IEnumerable<ExamGroupStudent>> GetExamGroupStudentsAsync(IEnumerable<Guid> examIds, CancellationToken cancellationToken)
        {
            return await _examStudentGroupRepo.ListAsync(new ExamGroupStudentsByExamIdIncludeAnswersSpec(examIds), cancellationToken);
        }

        #endregion

    }
}
