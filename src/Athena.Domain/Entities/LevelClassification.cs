namespace Athena.Domain.Entities
{
    public class LevelClassification : BaseEntity, IAggregateRoot
    {
        public Guid LevelId { get; private set; }
        public virtual Level Level { get; private set; } = default!;
        public Guid EducationClassificationId { get; private set; }
        public virtual EducationClassification EducationClassification { get; private set; } = default!;

        public virtual ICollection<Student> Students { get; private set; } = default!;

        public LevelClassification(Guid levelId, Guid educationClassificationId)
        {
            LevelId = levelId;
            EducationClassificationId = educationClassificationId;
        }

        public LevelClassification Update(Guid? levelId)
        {
            if (levelId.HasValue && levelId.Value != Guid.Empty && !LevelId.Equals(levelId.Value)) LevelId = levelId.Value;
            return this;
        }
    }
}
