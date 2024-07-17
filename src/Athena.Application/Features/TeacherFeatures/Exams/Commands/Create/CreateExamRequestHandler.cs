using Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Create
{
    public class CreateExamRequestHandler : IRequestHandler<CreateExamRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IReadRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;
        private readonly IRepository<Exam> _examRepo;
        private readonly IJobService _jobService;
        private readonly ICreateExamHandler _createExamHandler;
        private readonly ICreateExamDependancies _createExamDependancies;
        private readonly IStringLocalizer<CreateExamRequestHandler> _t;

        public CreateExamRequestHandler(
            ICurrentUser currentUser, 
            IReadRepository<Teacher> teacherRepo,
            IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo, 
            IRepository<Exam> examRepo,
            ICreateExamHandler createExamHandler,
            ICreateExamDependancies createExamDependancies,
            IJobService jobService,
            IStringLocalizer<CreateExamRequestHandler> t)
        {
            _currentUser = currentUser;
            _teacherRepo = teacherRepo; 
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
            _examRepo = examRepo;
            _createExamDependancies = createExamDependancies;
            _createExamHandler = createExamHandler;
            _jobService = jobService;
            _t = t;
        }

        public async Task<Guid> Handle(CreateExamRequest request, CancellationToken cancellationToken)
        {
            Guid businessId = _currentUser.GetBusinessId();
            Teacher teacher = await GetTeacerAsync(businessId, cancellationToken);
            TeacherCourseLevelYear teacherCourseLevelYear = await GetTeacherCourseLevelYearAsync(request, businessId, cancellationToken);

            double finalDegree = request.Sections.Sum(e => e.Questions.Sum(e => e.Degree));

            Exam newExam = new(request.Name, request.Description, ExamState.Upcoming, finalDegree, request.AllowedTime, request.PublishedDate, request.PublishedTime, false, request.IsPrime, false, teacherCourseLevelYear.Id, request.ExamTypeId, businessId);
            await _examRepo.AddAsync(newExam, cancellationToken);

            var ids = await _createExamDependancies.CreateExamDependanciesAsync(request, newExam.Id, businessId, cancellationToken);
            var sectionIds = await _createExamDependancies.CreateSectionsAsync(request, ids.ExamGroupStudentIds, newExam.Id,  businessId, cancellationToken);
            await _createExamDependancies.CreateStudentExamStatesAsync(sectionIds, ids.StudentIds, cancellationToken);

            CreateJobs(request, teacher, newExam, ids.StudentIds, cancellationToken);

            await _createExamHandler.CreateExamNotifications(teacher, newExam, ids.StudentIds, cancellationToken);
            return newExam.Id;
        }

        #region DataBase:

        private async Task<TeacherCourseLevelYear> GetTeacherCourseLevelYearAsync(CreateExamRequest request, Guid businessId, CancellationToken cancellationToken)
        {
            TeacherCourseLevelYear? teacherCourseLevelYear = await _teacherCourseLevelYearRepo.GetBySpecAsync(new OpenYearByBusinessIdAndTeacherCourseLevelIdSpec(businessId, request.LevelId), cancellationToken);
            _ = teacherCourseLevelYear ?? throw new ConflictException(_t["There are no open years yet."]);
            return teacherCourseLevelYear;
        }

        private async Task<Teacher> GetTeacerAsync(Guid businessId, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepo.GetByIdAsync(businessId, cancellationToken);
            _ = teacher ?? throw new NotFoundException(_t["Teacher Not Found!"]);
            return teacher;
        }

        #endregion

        #region Create Jobs:

        private void CreateJobs(CreateExamRequest request, Teacher teacher, Exam newExam, List<Guid> studentIds, CancellationToken cancellationToken)
        {
            DateTime publishTime = new(
                request.PublishedDate.Year,
                request.PublishedDate.Month,
                request.PublishedDate.Day,
                request.PublishedTime.Hours,
                request.PublishedTime.Minutes,
                0);

            TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            DateTime scheduledTime = TimeZoneInfo.ConvertTimeToUtc(publishTime, egyptTimeZone);

            // Convert the localDateTime to UTC using the UTC time zone
            DateTime activationDateTime = scheduledTime;
            DateTime deactivationDateTime = activationDateTime.AddMinutes(request.AllowedTime);

            _jobService.Schedule(() => _createExamHandler.ActivateExamJob(teacher, newExam.Id, studentIds, cancellationToken), activationDateTime);
            _jobService.Schedule(() => _createExamHandler.EndActiveExamJob(teacher, newExam.Id, cancellationToken), deactivationDateTime);
            _jobService.Schedule(() => _createExamHandler.CorrectExamJob(teacher, newExam.Id, cancellationToken), deactivationDateTime.AddMinutes(1));
        }

        #endregion

    }
}
