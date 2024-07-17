namespace Athena.Domain.Entities
{
    public class StudentTeacherRequest : AuditableEntity, IAggregateRoot
    {

        public Guid StudentId { get; private set; }
        public virtual Student Student { get; private set; } = null!;
        public Guid TeacherId { get; private set; }
        public virtual Teacher Teacher { get; private set; } = null!;
        public Guid GroupId { get; private set; }
        public virtual Group Group { get; private set; } = null!;
        public string State { get; private set; }
        public string? Message { get; private set; }


        public StudentTeacherRequest(Guid studentId, Guid teacherId, Guid groupId, string state, string? message, Guid businessId)
        {
            StudentId = studentId;
            TeacherId = teacherId;
            GroupId = groupId;
            State = state;
            Message = message;
            BusinessId = businessId;
        }

        public StudentTeacherRequest Update(Guid? groupId, string? state, string? message)
        {
            if (groupId is not null && GroupId.Equals(groupId) is not true) GroupId = (Guid)groupId;
            if (state is not null && State.Equals(state) is not true) State = state;
            if (message is not null && Message?.Equals(message) is not true) Message = message;
            return this;
        }


    }
}
