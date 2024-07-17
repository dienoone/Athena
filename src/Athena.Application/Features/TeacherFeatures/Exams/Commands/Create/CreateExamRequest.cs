using Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers;

namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Create
{
    // ToDo: Check ExamName if exist or not
    // ToDo: Add Teacher Cousre Levels:
    // levelId --> TeacherCourseLevelYear
    // ToDo: EndExam Session for signalR
    public class CreateExamRequest : IRequest<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int AllowedTime { get; set; }
        public DateTime PublishedDate { get; set; }
        public TimeSpan PublishedTime { get; set; }
        public bool IsPrime { get; set; }
        public Guid ExamTypeId { get; set; }
        public Guid LevelId { get; set; }


        public List<Guid> GroupIds { get; set; } = default!;

        public List<CreateExamRequestSectionHelper> Sections { get; set; } = default!;
    }
}
