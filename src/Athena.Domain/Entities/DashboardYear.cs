namespace Athena.Domain.Entities
{
    public class DashboardYear : BaseEntity, IAggregateRoot
    {
        public int Start { get; private set; }
        public DateTime? EndDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string State { get; private set; } = default!;
        public bool IsFinished { get; private set; }

        public DashboardYear(int start, DateTime? endDate, DateTime createdAt, string state, bool isFinished)
        {
            Start = start;
            EndDate = endDate;
            CreatedAt = createdAt;
            State = state;
            IsFinished = isFinished;

        }

        public DashboardYear Update(int? start, DateTime? endDate, string? state, bool? isFinished)
        {
            if (start != null && Start != start) Start = (int)start;
            if (endDate != null && EndDate != endDate) EndDate = endDate;
            if (state is not null && State?.Equals(state) is not true) State = state;
            if (isFinished != null && IsFinished != isFinished) IsFinished = (bool)isFinished;
            return this;
        }
    }
}
