namespace Athena.Domain.Entities
{
    public class StudentTeacherCommunication : AuditableEntity, IAggregateRoot
    {
        public Guid StudentId { get; private set; }
        public virtual Student Student { get; private set; } = default!;

        public Guid TeacherId { get; private set; }
        public virtual Teacher Teacher { get; private set; } = default!;

        public bool CanSendAgain { get; private set; }


        public StudentTeacherCommunication(Guid studentId, Guid teacherId, bool canSendAgain, Guid businessId)
        {
            StudentId = studentId;
            TeacherId = teacherId;
            CanSendAgain = canSendAgain;
            BusinessId = businessId;
        }

        public StudentTeacherCommunication Update(bool canSendAgain)
        {
            if (CanSendAgain != canSendAgain) CanSendAgain = canSendAgain;
            return this;
        }

    }
}
