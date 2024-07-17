using Athena.Application.Features.StudentFeatures.Exams.Dtos;
using Athena.Application.Features.StudentFeatures.Exams.Spec;
using Athena.Application.Features.TeacherFeatures.Exams.Spec;
using Athena.Domain.Common.Const;

namespace Athena.Application.Features.StudentFeatures.Exams.Queries
{
    public record GetActiveSectionsByExamIdAndUserIdRequest(Guid ExamId, Guid UserId) : IRequest<List<SmallActiveSectionDto>>;

    public class GetActiveSectionsByExamIdAndUserIdRequestHandler : IRequestHandler<GetActiveSectionsByExamIdAndUserIdRequest, List<SmallActiveSectionDto>>
    {
        private readonly IReadRepository<Section> _sectionRepo;
        private readonly IReadRepository<StudentSectionState> _studentSectionStateRepo;
        private readonly IRepository<ExamGroupStudent> _examGroupStudentRepo;

        public GetActiveSectionsByExamIdAndUserIdRequestHandler(
            IReadRepository<Section> sectionRepo, 
            IReadRepository<StudentSectionState> studentSectionStateRepo,
            IRepository<ExamGroupStudent> examGroupStudentRepo)
        {
            _sectionRepo = sectionRepo;
            _studentSectionStateRepo = studentSectionStateRepo;
            _examGroupStudentRepo = examGroupStudentRepo;
        }

        public async Task<List<SmallActiveSectionDto>> Handle(GetActiveSectionsByExamIdAndUserIdRequest request, CancellationToken cancellationToken)
        {
            var examGroupStudent = await _examGroupStudentRepo.GetBySpecAsync(new ExamGroupStudentByStudentIdAndExamIdSpec(request.UserId, request.ExamId), cancellationToken);
            examGroupStudent!.Update(null, true, null, null);
            await _examGroupStudentRepo.UpdateAsync(examGroupStudent, cancellationToken);

            var sections = await _sectionRepo.ListAsync(new SectionsIncludeQuestionsByExamIdSpec(request.ExamId), cancellationToken);
            return await MapSection(sections, request.UserId, cancellationToken);
        }

        private async Task<List<SmallActiveSectionDto>>  MapSection(List<Section> sections, Guid userId, CancellationToken cancellationToken)
        {
            List<SmallActiveSectionDto> sectionDtos = new();

            foreach(var section in sections)
            {
                var studentSectionState = await _studentSectionStateRepo.GetBySpecAsync(new StudentSectionStateBySectionIdAndUserIdSpec(section.Id, userId), cancellationToken);
                sectionDtos.Add(MapSection(section, studentSectionState!.State));
            }

            return sectionDtos;
        }

        private static SmallActiveSectionDto MapSection(Section section, string state)
        {
            return new()
            {
                Id = section.Id,
                Name = section.Name,
                Paragraph = section.Paragraph, 
                Degree = section.Questions.Sum(e => e.Degree),
                IsPrime = section.IsPrime,
                HasMCQ = section.Questions.Any(e => e.Type == QuestionTypes.MCQ),
                HasWritten = section.Questions.Any(e => e.Type == QuestionTypes.Written),
                State = state
            };
        }
    }
}
