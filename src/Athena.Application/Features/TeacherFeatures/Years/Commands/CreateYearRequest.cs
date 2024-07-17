using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.StudentFeatures.Home.Spec;
using Athena.Application.Features.TeacherFeatures.Years.Spec;
using Athena.Domain.Common.Const;
using System.Globalization;

namespace Athena.Application.Features.TeacherFeatures.Years.Commands
{
    // ToDo: Notifications for years
    public record CreateTeacherCoureLevelYear(Guid Id, int IntroFee, int MonthFee, CreateTeacherCoureLevelYearSemster Semster);
    
    public record CreateTeacherCoureLevelYearSemster(
        DateTime FristSemeterStartDate, 
        DateTime FristSemeterEndDate,
        DateTime SecondSemeterStartDate, 
        DateTime SecondSemeterEndDate);

    public class CreateYearRequest : IRequest<Guid>
    {
        public string State { get; set; } = default!;
        public List<CreateTeacherCoureLevelYear> TeacherCoureLevels { get; set; } = default!;
    }

    public class CreateYearRequestValidator : CustomValidator<CreateYearRequest>
    {
        public CreateYearRequestValidator(
            ICurrentUser currentUser,
            IReadRepository<Year> yearRepo,
            IReadRepository<TeacherCourseLevel> teacherCourseLevelRepo,
            IStringLocalizer<CreateYearRequestValidator> T)
        {
            RuleFor(e => e.State)
                .NotEmpty()
                .NotNull()
                .WithMessage((_, _) => T["Year State Can't be null."])
                .Must(state => IsValidYearState(state))
                .WithMessage((_, state) => T["Invalid Year State: {0}.", state])
                .MustAsync(async (state, ct) => await yearRepo.GetBySpecAsync(new YearByStateAndBusinessIdSpec(state, currentUser.GetBusinessId()), ct) is null)
                .WithMessage((_, state) => T["Can't add year with state {0}, because it is aleardy exist.", state]);

            RuleFor(e => e.TeacherCoureLevels)
                .MustAsync(async (_, teacherCourseLevels, ct) => await CheckTeacherCourseLevelIds(teacherCourseLevels, teacherCourseLevelRepo, T, ct));
        }

        // Helper method to check if the state is valid
        private static bool IsValidYearState(string state)
        {
            return state == YearStatus.Open || state == YearStatus.Preopen;
        }

        private static async Task<bool> CheckTeacherCourseLevelIds(List<CreateTeacherCoureLevelYear> teacherCourseLevels, IReadRepository<TeacherCourseLevel> teacherCourseLevelRepo, IStringLocalizer<CreateYearRequestValidator> T, CancellationToken ct)
        {
            if (teacherCourseLevels.Count < 1)
                throw new ConflictException(T["You Must Add Teacher Course Levels"]);

            foreach (var teacherCoureLevelYear in teacherCourseLevels)
            {
                var teacherCourseLevel = await teacherCourseLevelRepo.GetByIdAsync(teacherCoureLevelYear.Id, ct);
                _ = teacherCourseLevel ?? throw new NotFoundException(T["TeacherCourseLevel {0} Not Found!", teacherCoureLevelYear.Id]);

                if(!(teacherCoureLevelYear.Semster.FristSemeterStartDate < teacherCoureLevelYear.Semster.FristSemeterEndDate))
                    throw new ConflictException(T["FirstSemseterStartDate must be less than FristSemsterEndDate"]);

                if (!(teacherCoureLevelYear.Semster.SecondSemeterStartDate < teacherCoureLevelYear.Semster.SecondSemeterEndDate))
                    throw new ConflictException(T["SecondSemseterStartDate must be less than SecondSemsterEndDate"]);

                if (!(teacherCoureLevelYear.Semster.FristSemeterEndDate < teacherCoureLevelYear.Semster.SecondSemeterStartDate))
                    throw new ConflictException(T["FirstSemseterEndDate must be less than SecondSemsterStartDate"]);
            }
            return true;
        }
    }

    public class CreateYearRequestHandler : IRequestHandler<CreateYearRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IRepository<Year> _yearRepo;
        private readonly IReadRepository<DashboardYear> _dashboardYearRepo;
        private readonly IRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;
        private readonly IRepository<TeacherCourseLevelYearSemster> _teacherCourseLevelYearSemsterRepo;
        private readonly IReadRepository<GroupScadual> _groupScadualRepo;
        private readonly IJobService _jobService;
        private readonly INotificationSender _notificationSender;
        private readonly INotificationService _notificationService;
        private readonly ISignalRConnectionService _connectionService;

        public CreateYearRequestHandler(
            ICurrentUser currentUser, 
            IReadRepository<Teacher> teacherRepo,
            IRepository<Year> yearRepo, 
            IReadRepository<DashboardYear> dashboardYearRepo,
            IRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo,
            IRepository<TeacherCourseLevelYearSemster> teacherCourseLevelYearSemsterRepo,
            IReadRepository<GroupScadual> groupScadualRepo,
            IJobService jobService,
            INotificationSender notificationSender,
            INotificationService notificationService,
            ISignalRConnectionService connectionService)
        {
            _currentUser = currentUser;
            _teacherRepo = teacherRepo;
            _dashboardYearRepo = dashboardYearRepo;
            _yearRepo = yearRepo;
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
            _teacherCourseLevelYearSemsterRepo = teacherCourseLevelYearSemsterRepo;
            _groupScadualRepo = groupScadualRepo;
            _jobService = jobService;
            _notificationSender = notificationSender;
            _notificationService = notificationService;
            _connectionService = connectionService;
        }
        
        public async Task<Guid> Handle(CreateYearRequest request, CancellationToken cancellationToken)
        {
            Guid busniessId = _currentUser.GetBusinessId();
            var queryYear = await _dashboardYearRepo.GetBySpecAsync(new DashboardYearByStateSpec(request.State), cancellationToken);
            Year newYear = new(queryYear!.Id, request.State, true, busniessId);
            await _yearRepo.AddAsync(newYear, cancellationToken);

            foreach (var teacherCourseLevel in request.TeacherCoureLevels)
            {
                TeacherCourseLevelYear teacherCourseLevelYear = new(teacherCourseLevel.Id, newYear.Id, teacherCourseLevel.IntroFee, teacherCourseLevel.MonthFee, busniessId);
                await _teacherCourseLevelYearRepo.AddAsync(teacherCourseLevelYear, cancellationToken);

                TeacherCourseLevelYearSemster teacherCourseLevelYearSemster1 = new(Semster.FirstSemster, teacherCourseLevel.Semster.FristSemeterStartDate, teacherCourseLevel.Semster.FristSemeterEndDate, teacherCourseLevelYear.Id, busniessId);
                await _teacherCourseLevelYearSemsterRepo.AddAsync(teacherCourseLevelYearSemster1, cancellationToken);

                TeacherCourseLevelYearSemster teacherCourseLevelYearSemster2 = new(Semster.SecondSemster, teacherCourseLevel.Semster.SecondSemeterStartDate, teacherCourseLevel.Semster.SecondSemeterEndDate, teacherCourseLevelYear.Id, busniessId);
                await _teacherCourseLevelYearSemsterRepo.AddAsync(teacherCourseLevelYearSemster2, cancellationToken);
            }

            return newYear.Id;
        }

        public async Task SendGroupNotifications(TeacherCourseLevelYear teacherCourseLevelYear, CancellationToken cancellationToken)
        {
            var today = DateTime.Today.ToString("dddd", CultureInfo.InvariantCulture);
            var groupScaduals = await _groupScadualRepo.ListAsync(new GroupScadualsByDayAndTeacherCourseLevelYearId(today, teacherCourseLevelYear.Id));
            foreach(var scadual in groupScaduals)
            {


                //_jobService.Schedule()
            }
        }

        private async Task SendNotification(GroupScadual groupScadual, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepo.GetByIdAsync(groupScadual.BusinessId, cancellationToken);

            var EnGreeting = GetAmOrPM(groupScadual.StartTime) == "AM" ? "Good Morning" : "Good Evening";
            var ArGreeting = GetAmOrPM(groupScadual.StartTime) == "AM" ? (teacher!.Gender == "male" ? "صباح الخير, لديك" : "صباح الخير, لديكى") 
                    : (teacher!.Gender == "male" ? "مساء الخير, لديك" : "مساء الخير, لديكى");
            
            var EnGroup = $"";
            var ArGroup = $"";

            CreateNotificationWrapperRequest notification = new()
            {
                Type = ENotificationType.GroupSchedule.ToString(),
                Label = ENotificationLabel.Information.ToString(),
                EntityId = groupScadual.Group!.Id,
                NotifierId = teacher!.Id,
                BusinessId = teacher.BusinessId,
                Image = null,
                ArMessage = teacher!.Gender == "male" ? $"قبل الاستاذ {teacher.Name}" : $"قبلت الاستاذه {teacher.Name}",
                EnMessage = $"",
            };

            /*var notificationDto = await _notificationService.CreateNotficationAsync(notification, cancellationToken);
            var id = await _notificationService.CreateNotficationRecipientAsync(studentRequest.StudentId, notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUserAsync(notificationDto, id, cancellationToken);*/
        }

        private static string GetAmOrPM(TimeSpan time)
        {
            DateTime today = DateTime.Today.Add(time);
            string amOrPm = today.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            return amOrPm;
        }

        #region EndYearNotification:

        public void AlertEndCurrentYearJobs(Teacher teacher, Guid yearId, int start, DateTime SecondSemsterEndDate, CancellationToken cancellationToken)
        {
            DateTime queryDateTime = new(SecondSemsterEndDate.Year, SecondSemsterEndDate.Month, SecondSemsterEndDate.Day, 1, 0, 0);

            // Calculate the DateTime one week before
            DateTime oneWeekBefore = queryDateTime.AddDays(-7);
            _jobService.Schedule(() => AlertEndCurrentYearOneWeekNotification(teacher, yearId, start, cancellationToken), oneWeekBefore);

            // Calculate the DateTime 48 hours before
            DateTime fortyEightHoursBefore = queryDateTime.AddHours(-48);
            _jobService.Schedule(() => AlertEndCurrentYear48HoursNotification(teacher, yearId, start, cancellationToken), fortyEightHoursBefore);

            // Calculate the DateTime 24 hours before
            DateTime twentyFourHoursBefore = queryDateTime.AddDays(-1);
            _jobService.Schedule(() => AlertEndCurrentYear24HoursNotification(teacher, yearId, start, cancellationToken), twentyFourHoursBefore);
        }

        public async Task AlertEndCurrentYearOneWeekNotification(Teacher teacher, Guid yearId, int start, CancellationToken cancellationToken)
        {
            string arMessageOneWeek = teacher.Gender == "male"
                ? $"استاذ {teacher.Name.Split()[0]}&&اسبوع&&{start}/{start + 1}"
                : $"استاذه {teacher.Name.Split()[0]}&&اسبوع&&{start}/{start + 1}";

            string enMessageOneWeek = teacher.Gender == "male"
                ? $"Mr. {teacher.Name.Split()[0]}&&There is one week&&{start}/{start + 1}"
                : $"Mrs. {teacher.Name.Split()[0]}&&There is one week&&{start}/{start + 1}";

            await AlertEndCurrentYearNotification(teacher, yearId, arMessageOneWeek, enMessageOneWeek, cancellationToken);
        }

        public async Task AlertEndCurrentYear48HoursNotification(Teacher teacher, Guid yearId, int start, CancellationToken cancellationToken)
        {
            string arMessage48Hours = teacher.Gender == "male"
                ? $"استاذ {teacher.Name.Split()[0]}&&اسبوع&&{start}/{start + 1}"
                : $"استاذه {teacher.Name.Split()[0]}&&اسبوع&&{start}/{start + 1}";

            string enMessage48Hours = teacher.Gender == "male"
                ? $"Mr. {teacher.Name.Split()[0]}&&There is one week&&{start}/{start + 1}"
                : $"Mrs. {teacher.Name.Split()[0]}&&There is one week&&{start}/{start + 1}";

            await AlertEndCurrentYearNotification(teacher, yearId, arMessage48Hours, enMessage48Hours, cancellationToken);
        }

        public async Task AlertEndCurrentYear24HoursNotification(Teacher teacher, Guid yearId, int start, CancellationToken cancellationToken)
        {
            string arMessage24Hours = teacher.Gender == "male"
                ? $"استاذ {teacher.Name.Split()[0]}&&اسبوع&&{start}/{start + 1}"
                : $"استاذه {teacher.Name.Split()[0]}&&اسبوع&&{start}/{start + 1}";

            string enMessage24Hours = teacher.Gender == "male"
                ? $"Mr. {teacher.Name.Split()[0]}&&There is one week&&{start}/{start + 1}"
                : $"Mrs. {teacher.Name.Split()[0]}&&There is one week&&{start}/{start + 1}";

            await AlertEndCurrentYearNotification(teacher, yearId, arMessage24Hours, enMessage24Hours, cancellationToken);
        }

        private async Task AlertEndCurrentYearNotification(Teacher teacher, Guid yearId, string arMessage, string enMessage, CancellationToken cancellationToken)
        {
            CreateNotificationWrapperRequest notification = new()
            {
                Type = ENotificationType.AlertEndCurrentYear.ToString(),
                Label = ENotificationLabel.Warning.ToString(),
                EntityId = yearId,
                NotifierId = teacher!.Id,
                BusinessId = teacher.BusinessId,
                Image = teacher.ImagePath,
                ArMessage = arMessage,
                EnMessage = enMessage,
            };

            var notificationDto = await _notificationService.CreateNotificationThreePlaceHoldersAsync(notification, cancellationToken);
            var connectionIds = await _notificationService.CreateNotficationRecipientAsync(teacher.Id, notificationDto.Id, notification.BusinessId, cancellationToken);
            await _notificationSender.SendToUsersAsync(notificationDto, connectionIds, cancellationToken);
        }

        #endregion
    }



}
