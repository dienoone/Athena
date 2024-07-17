namespace Athena.Domain.Entities
{
    public class StudentSectionState : BaseEntity, IAggregateRoot
    {
        public string State { get; private set; }

        public Guid StudentId { get; private set; }
        public Guid SectionId { get; private set; }
        public Section Section { get; private set; } = default!;

        public StudentSectionState(Guid studentId, Guid sectionId, string state)
        {
            StudentId = studentId;
            SectionId = sectionId;
            State = state;
        }

        public StudentSectionState Update(string? state)
        {
            if (state is not null && State.Equals(state) is not true) State = state;
            return this;
        }

    }
}
