namespace Athena.Domain.Entities
{
    public class TeacherContact : AuditableEntity, IAggregateRoot
    {
        public string Contact { get; private set; } = default!;
        public string Data { get; private set; } = default!;
        public Guid TeacherId { get; private set; }
        public Teacher Teacher { get; private set; } = null!;

        public TeacherContact(string contact, string data, Guid teacherId, Guid businessId)
        {
            Contact = contact;
            Data = data;
            TeacherId = teacherId;
            BusinessId = businessId;
        }

        public TeacherContact Update(string? data)
        {
            if (data is not null && Data.Equals(data) is not true) Data = data;
            return this;
        }

    }
}
