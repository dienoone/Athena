namespace Athena.Domain.Entities
{
    public class HeadQuarterPhone : AuditableEntity, IAggregateRoot
    {
        public string Phone { get; private set; } = default!;
        public Guid HeadQuarterId { get; private set; }
        public virtual HeadQuarter HeadQuarter { get; private set; } = default!;

        public HeadQuarterPhone(string phone, Guid headQuarterId, Guid businessId)
        {
            Phone = phone;
            HeadQuarterId = headQuarterId;
            BusinessId = businessId;
        }

        public HeadQuarterPhone Update(string? phone)
        {
            if (phone is not null && Phone?.Equals(phone) is not true) Phone = phone;
            return this;
        }
    }
}
