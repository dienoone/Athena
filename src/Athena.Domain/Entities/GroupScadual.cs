namespace Athena.Domain.Entities
{
    public class GroupScadual : AuditableEntity, IAggregateRoot
    {
        public string Day { get; private set; } = default!;
        public TimeSpan StartTime { get; set; } = default!;
        public TimeSpan EndTime { get; private set; } = default!;
        public string? Description { get; private set; }
        public Guid GroupId { get; private set; }
        public virtual Group Group { get; private set; } = default!;

        public GroupScadual(string day, TimeSpan startTime, TimeSpan endTime, string? description, Guid groupId, Guid businessId) =>
            (Day, StartTime, EndTime, Description, GroupId, BusinessId) = (day, startTime, endTime, description, groupId, businessId);

        public GroupScadual Update(string? day, TimeSpan? startTime, TimeSpan? endTime)
        {
            if (day is not null && Day.Equals(day) is not true) Day = day;
            if (startTime is not null && StartTime.Equals(startTime) is not true) StartTime = (TimeSpan)startTime;
            if (endTime is not null && EndTime.Equals(endTime) is not true) EndTime = (TimeSpan)endTime;
            return this;
        }
        

    }
}
