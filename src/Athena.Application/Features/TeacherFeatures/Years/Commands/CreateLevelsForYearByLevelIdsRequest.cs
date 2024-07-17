using Athena.Application.Features.TeacherFeatures.Years.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Years.Commands
{
    public record CreateLevelsForYearByLevelIds(Guid Id, int IntroFee, int MonthFee, CreateTeacherCoureLevelYearSemster Semster) : IRequest<Guid>;

    public class CreateLevelsForYearByLevelIdsRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public List<CreateLevelsForYearByLevelIds> Levels { get; set; } = default!;
    }

    public class CreateLevelsForYearByLevelIdsRequestValidator : CustomValidator<CreateLevelsForYearByLevelIdsRequest>
    {
        public CreateLevelsForYearByLevelIdsRequestValidator(
            IReadRepository<Year> yearRepo, 
            IReadRepository<TeacherCourseLevel> levelRepo, 
            IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo,
            IStringLocalizer<CreateLevelsForYearByLevelIdsRequestValidator> T)
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (id, ct) => await yearRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => T["Year {0} Not Found!", id]);

            RuleFor(e => e.Levels)
                .MustAsync(async (request, teacherCourseLevels, ct) => 
                await CheckTeacherCourseLevelIds(request.Id,teacherCourseLevels, levelRepo, teacherCourseLevelYearRepo,T, ct));
        }

        private static async Task<bool> CheckTeacherCourseLevelIds(
            Guid yearId,
            List<CreateLevelsForYearByLevelIds> teacherCourseLevels,
            IReadRepository<TeacherCourseLevel> teacherCourseLevelRepo,
            IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo,
            IStringLocalizer<CreateLevelsForYearByLevelIdsRequestValidator> T, 
            CancellationToken ct)
        {
            if (teacherCourseLevels.Count < 1)
                throw new ConflictException(T["You Must Add Teacher Course Levels"]);

            foreach (var teacherCoureLevelYear in teacherCourseLevels)
            {
                var teacherCourseLevel = await teacherCourseLevelRepo.GetByIdAsync(teacherCoureLevelYear.Id, ct);
                _ = teacherCourseLevel ?? throw new NotFoundException(T["TeacherCourseLevel {0} Not Found!", teacherCoureLevelYear.Id]);

                var isExist = await teacherCourseLevelYearRepo.GetBySpecAsync(new TeacherCourseLevelYearByYearIdAndTeacherCourseLevelIdSpec(yearId, teacherCourseLevel.Id), ct);
                if (!(isExist == null))
                    throw new ConflictException(T["TeacherCourseLevelYear wiht TeacherCouserLevel {0} already exist!", teacherCourseLevel.Id]);

                if (!(teacherCoureLevelYear.Semster.FristSemeterStartDate < teacherCoureLevelYear.Semster.FristSemeterEndDate))
                    throw new ConflictException(T["FirstSemseterStartDate must be less than FristSemsterEndDate"]);

                if (!(teacherCoureLevelYear.Semster.SecondSemeterStartDate < teacherCoureLevelYear.Semster.SecondSemeterEndDate))
                    throw new ConflictException(T["SecondSemseterStartDate must be less than SecondSemsterEndDate"]);

                if (!(teacherCoureLevelYear.Semster.FristSemeterEndDate < teacherCoureLevelYear.Semster.SecondSemeterStartDate))
                    throw new ConflictException(T["FirstSemseterEndDate must be less than SecondSemsterStartDate"]);
            }
            return true;
        }
    }

    public class CreateLevelsForYearByLevelIdsRequestHandler : IRequestHandler<CreateLevelsForYearByLevelIdsRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;
        private readonly IRepository<TeacherCourseLevelYearSemster> _teacherCourseLevelYearSemsterRepo;

        public CreateLevelsForYearByLevelIdsRequestHandler(
            ICurrentUser currentUser, 
            IRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo,
            IRepository<TeacherCourseLevelYearSemster> teacherCourseLevelYearSemsterRepo)
        {
            _currentUser = currentUser;
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
            _teacherCourseLevelYearSemsterRepo = teacherCourseLevelYearSemsterRepo;
        }

        public async Task<Guid> Handle(CreateLevelsForYearByLevelIdsRequest request, CancellationToken cancellationToken)
        {
            Guid busniessId = _currentUser.GetBusinessId();

            foreach (var teacherCourseLevel in request.Levels)
            {
                TeacherCourseLevelYear teacherCourseLevelYear = new(teacherCourseLevel.Id, request.Id, teacherCourseLevel.IntroFee, teacherCourseLevel.MonthFee, busniessId);
                await _teacherCourseLevelYearRepo.AddAsync(teacherCourseLevelYear, cancellationToken);

                TeacherCourseLevelYearSemster teacherCourseLevelYearSemster1 = new(Semster.FirstSemster, teacherCourseLevel.Semster.FristSemeterStartDate, teacherCourseLevel.Semster.FristSemeterEndDate, teacherCourseLevelYear.Id, busniessId);
                await _teacherCourseLevelYearSemsterRepo.AddAsync(teacherCourseLevelYearSemster1, cancellationToken);

                TeacherCourseLevelYearSemster teacherCourseLevelYearSemster2 = new(Semster.SecondSemster, teacherCourseLevel.Semster.SecondSemeterStartDate, teacherCourseLevel.Semster.SecondSemeterEndDate, teacherCourseLevelYear.Id, busniessId);
                await _teacherCourseLevelYearSemsterRepo.AddAsync(teacherCourseLevelYearSemster2, cancellationToken);
            }

            return request.Id;
        }
    }


}
