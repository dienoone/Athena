namespace Athena.Domain.Entities
{
    public class ExamStudentAnswer : AuditableEntity, IAggregateRoot
    {
        public Guid ExamGroupStudentId { get; private set; }
        public virtual ExamGroupStudent ExamGroupStudent { get; private set; } = default!;

        public Guid QuestionId { get; private set; }
        public virtual Question Question { get; private set; } = default!;

        public Guid? QuestionChoiceId { get; private set; }
        public QuestionChoice? QuestionChoice { get; private set; }

        public string? Answer { get; private set; }
        public double Degree { get; private set; }
        public bool IsCorrected { get; private set; }
        public bool IsAnswered { get; private set; }

        public ExamStudentAnswer(Guid examGroupStudentId, Guid questionId, Guid? questionChoiceId, string? answer, double degree, bool isCorrected, bool isAnswered, Guid businessId)
        {
            ExamGroupStudentId = examGroupStudentId;
            QuestionId = questionId;
            QuestionChoiceId = questionChoiceId;
            Answer = answer;
            Degree = degree;
            IsCorrected = isCorrected;
            IsAnswered = isAnswered;
            BusinessId = businessId;
        }

        public ExamStudentAnswer Update(Guid? questionChoiceId, string? answer, double? degree, bool? isCorrected, bool? isAnswered)
        {
            if (answer is not null && Answer?.Equals(answer) is not true) Answer = answer;
            if (degree is not null && Degree.Equals(degree) is not true) Degree = (double)degree;
            if (questionChoiceId.HasValue && questionChoiceId.Value != Guid.Empty && !QuestionChoiceId.Equals(questionChoiceId.Value))
                QuestionChoiceId = questionChoiceId.Value;
            if (isCorrected is not null && IsCorrected != isCorrected) IsCorrected = (bool)isCorrected;
            if (isAnswered is not null && IsAnswered != isAnswered) IsAnswered = (bool)isAnswered;

            return this;
        }
    }
}
