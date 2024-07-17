namespace Athena.Domain.Entities
{
    public class Attendence : AuditableEntity, IAggregateRoot
    {
        public DateTime Date { get; private set; }
        public string Month { get; private set; } = default!;

        public bool IsFinished { get; private set; }
        public Guid TeacherCourseLevelYearId { get; private set; }

        public TeacherCourseLevelYear TeacherCourseLevelYear { get; private set; } = default!;

        public Attendence(DateTime date, string month, bool isFinished, Guid teacherCourseLevelYearId, Guid businessId)
        {
            Date = date;
            Month = month;
            IsFinished = isFinished;
            TeacherCourseLevelYearId = teacherCourseLevelYearId;
            BusinessId = businessId;
        }

        public Attendence Update(bool? isFinished)
        {
            if (isFinished is not null && IsFinished != isFinished) IsFinished = (bool)isFinished; 
            return this;
        }
    }
}
