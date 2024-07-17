using Athena.Application.Features.StudentFeatures.Exams.Dtos;
using Athena.Application.Features.StudentFeatures.Exams.Spec;
using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Exams.Queries
{
    // ToDo: Add Check If State Is Active:
    public record GetActiveExamByIdRequest(Guid Id, Guid UserId) : IRequest<ActiveExamDto>;

    public class GetActiveExamByIdRequestHandler : IRequestHandler<GetActiveExamByIdRequest, ActiveExamDto>
    {
        private readonly IReadRepository<Exam> _examRepo;
        private readonly IReadRepository<Section> _sectionRepo;
        private readonly IReadRepository<Teacher> _teacherRepo;
        private readonly IRepository<ExamGroupStudent> _examGroupStudentRepo;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<GetActiveExamByIdRequestHandler> _t;

        public GetActiveExamByIdRequestHandler(
            IReadRepository<Exam> examRepo, 
            IReadRepository<Section> sectionRepo,
            IReadRepository<Teacher> teacherRepo, 
            IRepository<ExamGroupStudent> examGroupStudentRepo,
            IUserService userService, 
            IStringLocalizer<GetActiveExamByIdRequestHandler> t)
        {
            _examRepo = examRepo;
            _sectionRepo = sectionRepo;
            _teacherRepo = teacherRepo;
            _examGroupStudentRepo = examGroupStudentRepo;
            _userService = userService;
            _t = t;
        }

        public async Task<ActiveExamDto> Handle(GetActiveExamByIdRequest request, CancellationToken cancellationToken)
        {
            var exam = await _examRepo.GetBySpecAsync(new ExamDetailByIdSpec(request.Id), cancellationToken);
            _ = exam ?? throw new NotFoundException(_t["Exam {0} Not Found!", request.Id]);

            var teacher = await _teacherRepo.GetBySpecAsync(new TeacherByBusinessIdIncludeCourseSpec(exam.BusinessId), cancellationToken);
            var teacherUser = await _userService.GetAsync(teacher!.Id.ToString(), cancellationToken);

            var sections = await _sectionRepo.ListAsync(new SectionsDetailByExamIdSpec(request.Id), cancellationToken);
            List<SmallActiveSectionDto> sectionDetailDtos = new();

            foreach (var section in sections)
            {
                sectionDetailDtos.Add(MapSection(section));
            }

            ActiveExamDto dto = MapExam(exam);
            dto.Course = teacher.Course.Name;
            dto.TeacherName = teacherUser.FirstName + " " + teacherUser.LastName;
            dto.TeacherImage = teacher.ImagePath;
            dto.NumberOfSections = sections.Count;
            //dto.Sections = sectionDetailDtos;

            return dto;
        }

        private static ActiveExamDto MapExam(Exam exam)
        {
            return new()
            {
                Id = exam.Id,
                Name = exam.Name,
                ExamType = exam.ExamType.Name,
                PublishedDate = exam.PublishedDate,
                PublishedTime = exam.PublishedTime,
                AllowedTime = exam.AllowedTime,
                CreatedAt = exam.CreatedOn,
                FinalDegree = exam.FinalDegree,
                State = exam.State,
                IsPrime = exam.IsPrime,
            };
        }

        private static SmallActiveSectionDto MapSection(Section section)
        {
            List<ActiveImageDetailDto> Images = new();

            if (section.SectionImages != null)
            {
                foreach (var image in section.SectionImages)
                {
                    ActiveImageDetailDto imageDetailDto = new()
                    {
                        Id = image.Id,
                        Index = image.Index,
                        Image = image.Image,
                    };
                    Images.Add(imageDetailDto);
                }
            }

            return new()
            {
                Id = section.Id,
                Name = section.Name,
                Paragraph = section.Paragraph,
                Degree = section.Degree,
                IsPrime = section.IsPrime,
                //Images = Images.Count > 0 ? Images : null,
            };
        }
        
    }


}
