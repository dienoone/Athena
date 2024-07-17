using Athena.Application.Features.StudentFeatures.Exams.Dtos;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Exams.Queries
{
    public record GetUpcomingExamByIdRequest(Guid Id) : IRequest<UpcomingExamDto>;

    public class GetUpcomingExamByIdRequestHandler : IRequestHandler<GetUpcomingExamByIdRequest, UpcomingExamDto>
    {
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IStringLocalizer<GetUpcomingExamByIdRequestHandler> _t;

        public GetUpcomingExamByIdRequestHandler(IReadRepository<Exam> examRepo, IStringLocalizer<GetUpcomingExamByIdRequestHandler> t)
        {
            _examRepo = examRepo;
            _t = t;
        }

        public async Task<UpcomingExamDto> Handle(GetUpcomingExamByIdRequest request, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", request.Id]);

            if (!exam.State.Equals(ExamState.Upcoming))
                throw new ConflictException(_t["Exam {0} State Not upcoming!", request.Id]);

            return new() 
            { 
                Id = exam.Id,
                Name = exam.Name,
                PublishedDate = exam.PublishedDate,
                PublishedTime = exam.PublishedTime
            };
        }
    }


}
