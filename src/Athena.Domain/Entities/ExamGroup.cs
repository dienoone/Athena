namespace Athena.Domain.Entities
{
    public class ExamGroup : AuditableEntity, IAggregateRoot
    {
        public Guid GroupId { get; private set; }
        public virtual Group Group { get; private set; } = default!;

        public Guid ExamId { get; private set; }
        public virtual Exam Exam { get; private set; } = default!;

        public bool State { get; private set; }
        public bool IsReady { get; private set; }

        public virtual ICollection<ExamGroupStudent> ExamGroupStudents { get; private set; } = default!;

        public ExamGroup(Guid groupId, Guid examId, bool state, bool isReady, Guid businessId)
        {
            GroupId = groupId;
            ExamId = examId;
            State = state;
            IsReady = isReady;
            BusinessId = businessId;
        }

        public ExamGroup Update(bool? state, bool? isReady)
        {
            if (state is not null && State.Equals(state) is not true) State = (bool)state;
            if (isReady is not null && IsReady.Equals(isReady) is not true) IsReady = (bool)isReady;
            return this;
        }
    }
}
