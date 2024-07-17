namespace Athena.Domain.Entities
{
    public class ExamGroupStudent : AuditableEntity, IAggregateRoot
    {
        public Guid ExamGroupId { get; private set; }
        public virtual ExamGroup ExamGroup { get; private set; } = default!;

        public Guid GroupStudentId { get; private set; }
        public virtual GroupStudent GroupStudent { get; private set; } = default!;

        public string ExamDegreeState { get; private set; } = default!;
        public bool State { get; private set; }
        public double Degree { get; private set; }
        public double Points { get; private set; }

        public virtual ICollection<ExamStudentAnswer> ExamStudentAnswers { get; private set; } = default!;

        public ExamGroupStudent(Guid examGroupId, Guid groupStudentId, string examDegreeState, bool state, double degree, double points, Guid businessId)
        {
            ExamGroupId = examGroupId;
            GroupStudentId = groupStudentId;
            ExamDegreeState = examDegreeState;
            State = state;
            Degree = degree;
            Points = points;
            BusinessId = businessId;
        }

        public ExamGroupStudent Update(string? examDegreeState, bool? state, double? degree, double? points) 
        {
            if (examDegreeState is not null && ExamDegreeState.Equals(examDegreeState) is not true) ExamDegreeState = examDegreeState;
            if (state is not null && State.Equals(state) is not true) State = (bool)state;
            if (degree is not null && Degree.Equals(degree) is not true) Degree = (double)degree;
            if (points is not null && Points.Equals(points) is not true) Points = (double)points;
            return this;
        }
    }
}
