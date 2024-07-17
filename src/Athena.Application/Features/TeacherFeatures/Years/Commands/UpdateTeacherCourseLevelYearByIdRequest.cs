using Athena.Domain.Common.Const;

namespace Athena.Application.Features.TeacherFeatures.Years.Commands
{
    // ToDo: Condition for Start and end
    public record UpdateTeacherCourseLevelYearSemster(Guid Id, DateTime StartDate, DateTime EndDate);

    public class UpdateTeacherCourseLevelYearByIdRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public int IntroFee { get; set; }
        public int MonthFee { get; set; }

        public List<UpdateTeacherCourseLevelYearSemster>? Semsters { get; set; }

    }

    public class UpdateTeacherCourseLevelYearByIdRequestValidator : CustomValidator<UpdateTeacherCourseLevelYearByIdRequest>
    {
        public UpdateTeacherCourseLevelYearByIdRequestValidator(IReadRepository<TeacherCourseLevelYear> levelYearRepo, 
                IReadRepository<TeacherCourseLevelYearSemster> semsterRepo,
                IStringLocalizer<UpdateTeacherCourseLevelYearByIdRequestValidator> T)
        {
            RuleFor(e => e.Id)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (id, ct) => await levelYearRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => T["TeacherCourseLevelYear {0} Not Found!", id]);

            RuleFor(e => e.MonthFee)
                .GreaterThan(1)
                .WithMessage((_, monthFee) => T["MonthFee {0} Must be greater than one!", monthFee]);

            RuleFor(e => e.IntroFee)
                .GreaterThan(1)
                .WithMessage((_, introFee) => T["IntroFee {0} Must be greater than one!", introFee]);

            RuleFor(e => e.Semsters)
                .MustAsync(async (_, semsters, ct) => await CheckSemsters(semsters, semsterRepo, T, ct));
        }

        private static async Task<bool> CheckSemsters(List<UpdateTeacherCourseLevelYearSemster>? semsters,
            IReadRepository<TeacherCourseLevelYearSemster> semsterRepo,
                IStringLocalizer<UpdateTeacherCourseLevelYearByIdRequestValidator> T,
                CancellationToken cancellationToken)
        {
            if(semsters != null)
            {
                foreach(var semster in semsters)
                {
                    var querySemster = await semsterRepo.GetByIdAsync(semster.Id, cancellationToken);
                    _ = querySemster ?? throw new NotFoundException(T["Semster {0} Not Found!", semster.Id]);
                }
            }

            return true;
        }

    }

    public class UpdateTeacherCourseLevelYearByIdRequestHandler : IRequestHandler<UpdateTeacherCourseLevelYearByIdRequest, Guid>
    {
        private readonly IRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;
        private readonly IRepository<TeacherCourseLevelYearSemster> _teacherCourseLevelYearSemsterRepo;
        private readonly IStringLocalizer<UpdateTeacherCourseLevelYearByIdRequestHandler> _t;

        public UpdateTeacherCourseLevelYearByIdRequestHandler(IRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo, 
            IRepository<TeacherCourseLevelYearSemster> teacherCourseLevelYearSemsterRepo,
            IStringLocalizer<UpdateTeacherCourseLevelYearByIdRequestHandler> t)
        {
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
            _teacherCourseLevelYearSemsterRepo = teacherCourseLevelYearSemsterRepo;
            _t = t;
        }

        public async Task<Guid> Handle(UpdateTeacherCourseLevelYearByIdRequest request, CancellationToken cancellationToken)
        {
            var teacherCourseLevelYear = await _teacherCourseLevelYearRepo.GetByIdAsync(request.Id, cancellationToken);

            if(request.Semsters != null)
            {
                foreach (var semster in request.Semsters)
                {
                    var teacherCourseLevelYearSemster = await _teacherCourseLevelYearSemsterRepo.GetByIdAsync(semster.Id, cancellationToken);
                    teacherCourseLevelYearSemster!.Update(semster.StartDate, semster.EndDate);

                }
            }

            teacherCourseLevelYear!.Update(request.IntroFee, request.MonthFee);
            await _teacherCourseLevelYearRepo.UpdateAsync(teacherCourseLevelYear, cancellationToken);

            return request.Id;
        }
    }
}
