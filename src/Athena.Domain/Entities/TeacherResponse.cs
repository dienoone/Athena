namespace Athena.Domain.Entities
{
    public class TeacherResponse : AuditableEntity, IAggregateRoot
    {
        public Guid StudentTeacherRequestPhaseId { get; private set; }
        public StudentTeacherRequest StudentTeacherRequestPhase { get; private set; } = null!;

        public string? Message { get; set; }

        public TeacherResponse(Guid studentTeacherRequestPhaseId, string? message)
        {
            StudentTeacherRequestPhaseId = studentTeacherRequestPhaseId;
            Message = message;
        }

        public TeacherResponse Update(string? message)
        {
            if (message is not null && Message?.Equals(message) is not true) Message = message;
            return this;
        }


    }
}
