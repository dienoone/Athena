namespace Athena.Domain.Entities
{
    public class Question : AuditableEntity, IAggregateRoot
    {
        public int Index { get; private set; }
        public string Name { get; private set; } = default!;
        public string Type { get; private set; } = default!;
        public string? Answer { get; set; }
        public double Degree { get; private set; }
        public bool IsPrime { get; private set; } 
        public Guid SectionId { get; private set; }
        public virtual Section Section { get; private set; } = default!;

        public virtual ICollection<QuestionImage>? QuestionImages { get; private set; } 
        public virtual ICollection<QuestionChoice>? QuestionChoices { get; private set; }
        public virtual ICollection<ExamStudentAnswer> ExamStudentAnswers { get; private set; } = default!;

        public Question(int index, string name, string type, string? answer, double degree, bool isPrime, Guid sectionId, Guid businessId)
        {
            Index = index;
            Name= name;
            Type= type;
            Answer = answer;
            Degree= degree;
            IsPrime= isPrime;
            SectionId= sectionId;
            BusinessId= businessId;
        }

        public Question Update(int? index, string? name, string? type, string? answer, double? degree, bool? isPrime)
        {
            if (index is not null && Index.Equals(index) is not true) Index = (int)index;
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (type is not null && Type?.Equals(type) is not true) Type = type;
            if (answer is not null && Answer?.Equals(answer) is not true) Answer = answer;
            if (degree is not null && Degree.Equals(degree) is not true) Degree = (double)degree;
            if (isPrime is not null && IsPrime.Equals(isPrime) is not true) IsPrime = (bool)isPrime;
            return this;
        }
    }
}
