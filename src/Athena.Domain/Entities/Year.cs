namespace Athena.Domain.Entities
{
    public class Year : AuditableEntity, IAggregateRoot
    {
        public Guid DashboardYearId { get; private set; }
        public DashboardYear DashboardYear { get; private set; } = null!;

        public string YearState { get; private set; } = default!;
        public bool State { get; private set; }

        public virtual ICollection<TeacherCourseLevelYear> TeacherCourseLevelYears { get; private set; } = default!;

        public Year(Guid dashboardYearId, string yearState, bool state, Guid businessId)
        {
            DashboardYearId = dashboardYearId;
            YearState = yearState;
            State = state;
            BusinessId = businessId;
        }

        public Year Update(string? yearState, bool? state)
        {
            if (yearState is not null && YearState?.Equals(yearState) is not true) YearState = yearState;
            if (state != null && State != state) State = (bool)state;
            return this;
        }
    }
}
