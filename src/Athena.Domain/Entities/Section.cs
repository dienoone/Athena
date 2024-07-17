namespace Athena.Domain.Entities
{
    public class Section : AuditableEntity, IAggregateRoot
    {
        public int Index { get; private set; }
        public string Name { get; private set; } = default!;
        public string? Paragraph { get; private set; }
        public double Degree { get; private set; }
        public bool IsPrime { get; private set; }
        public int? Time { get; private set; }
        public Guid ExamId { get; private set; }
        public virtual Exam Exam { get; private set; } = default!;

        public virtual ICollection<SectionImage>? SectionImages { get; private set; } 
        public virtual ICollection<Question> Questions { get; private set; } = default!;

        public Section(int index, string name, string? paragraph, double degree, bool isPrime, int? time, Guid examId, Guid businessId)
        {
            Index = index;
            Name = name;
            Paragraph = paragraph;
            Degree = degree;
            IsPrime = isPrime;
            Time = time;
            ExamId = examId;
            BusinessId = businessId;
        }

        public Section Update(int? index, string? name, string? paragraph, double? degree, bool? isPrime, int? time)
        {
            if (index is not null && Index.Equals(index) is not true) Index = (int)index;
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (paragraph is not null && Paragraph?.Equals(paragraph) is not true) Paragraph = paragraph;
            if (degree is not null && Degree.Equals(degree) is not true) Degree = (int)degree;
            if (isPrime is not null && IsPrime.Equals(isPrime) is not true) IsPrime = (bool)isPrime;
            if (time is not null && Time.Equals(time) is not true) Time = (int)time;
            return this;
        }
    }
}
