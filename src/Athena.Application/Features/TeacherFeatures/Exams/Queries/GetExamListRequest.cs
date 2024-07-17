using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;

namespace Athena.Application.Features.TeacherFeatures.Exams.Queries
{
    public record GetExamListRequest() : IRequest<List<ExamListDto>>;
    public class GetExamListRequestHandler : IRequestHandler<GetExamListRequest, List<ExamListDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IReadRepository<Exam> _examRepo;

        public GetExamListRequestHandler(ICurrentUser currentUser, IReadRepository<Exam> examRepo)
        {
            _currentUser = currentUser;
            _examRepo = examRepo;
        }

        public async Task<List<ExamListDto>> Handle(GetExamListRequest request, CancellationToken cancellationToken)
        {
            var exams = await _examRepo.ListAsync(new ExamListByBusinessIdSpec(_currentUser.GetBusinessId()), cancellationToken);
            return exams.OrderByDescending(e => e.CreatedOn).Adapt<List<ExamListDto>>();
        }
    }

}
