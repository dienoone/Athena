using Athena.Application.Features.DashboardFeatures.ExamTypes.Dtos;
using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;

namespace Athena.Application.Features.TeacherFeatures.Exams.Queries
{
    // YearsWithLevelsRequiredByBusniessIdSpec
    public record GetCreateExamRequiredRequest() : IRequest<CreateExamRequiredDto>;
    public class GetCreateExamRequiredRequestHandler : IRequestHandler<GetCreateExamRequiredRequest, CreateExamRequiredDto>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<ExamType> _examTypeRepo;
        private readonly IReadRepository<TeacherCourseLevelYear> _teacherCourseLevelYearRepo;

        public GetCreateExamRequiredRequestHandler(ICurrentUser currentUser, IReadRepository<ExamType> examTypeRepo, 
            IReadRepository<TeacherCourseLevelYear> teacherCourseLevelYearRepo)
        {
            _currentUser = currentUser;
            _examTypeRepo = examTypeRepo;
            _teacherCourseLevelYearRepo = teacherCourseLevelYearRepo;
        }

        public async Task<CreateExamRequiredDto> Handle(GetCreateExamRequiredRequest request, CancellationToken cancellationToken)
        {
            var examTypes = await _examTypeRepo.ListAsync(cancellationToken);
            var levels = await _teacherCourseLevelYearRepo.ListAsync(new TeacherCourseLevelsByBusinessIdSpec(_currentUser.GetBusinessId()), cancellationToken);

            return new() 
            {
                ExamTypes = examTypes?.OrderBy(e => e.Index).Adapt<List<ExamTypeDto>>(),
                Levels = levels?.Adapt<List<TeacherCourseLevelYearRequiredToCreateExamDto>>()
            };
        }
    }


}
