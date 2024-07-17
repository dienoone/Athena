namespace Athena.Domain.Entities
{
    public class CodeHelper : BaseEntity, IAggregateRoot
    {
        public long Count { get; private set; }

        public CodeHelper(long count)
        {
            Count = count;
        }

        public CodeHelper Update(long? count)
        {
            if(count is not null && Count != count) Count = (long)count;
            return this;
        }
    }
}
