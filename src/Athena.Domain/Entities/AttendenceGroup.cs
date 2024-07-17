namespace Athena.Domain.Entities
{
    public class AttendenceGroup : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; } = default!;
        public Guid AttendenceId { get; private set; }
        public Attendence Attendence { get; private set; } = default!;

        public Guid GroupId { get; private set; }
        public Group Group { get; private set; } = default!;

        public bool IsStarted { get; private set; }
        public bool IsFinished { get; private set; }

        public AttendenceGroup(string name, Guid attendenceId, Guid groupId, bool isStarted, bool isFinished, Guid busniessId)
        {
            Name = name;
            AttendenceId = attendenceId;
            GroupId = groupId;
            IsStarted = isStarted;
            IsFinished = isFinished;
            BusinessId = busniessId;
        }

        public AttendenceGroup Update(string? name, bool? isStarted, bool? isFinished)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (isStarted is not null && IsStarted != isStarted) IsStarted = (bool)isStarted;
            if (isFinished is not null && IsFinished != isFinished) IsFinished = (bool)isFinished;
            return this;
        }
    }
}
