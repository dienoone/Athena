namespace Athena.Domain.Entities
{
    public class ExamType : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; } = default!;
        public int Index { get; set; }

        public virtual ICollection<Exam> Exams { get; private set; } = default!;

        public ExamType(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public ExamType Update(string? name, int? index)
        {
            if(name is not null && Name?.Equals(name) is not true) Name = name;
            if (index is not null && index != Index) Index = (int)index;
            return this;
        }
    }
}
