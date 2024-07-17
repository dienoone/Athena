namespace Athena.Domain.Entities
{
    public class Level : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; } = default!;

        public int Index { get; set; }

        public virtual ICollection<LevelClassification> LevelClassifications { get; private set; } = default!;
        public virtual ICollection<TeacherCourseLevel> TeacherCourseLevels { get; private set; } = null!;

        public Level(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public Level Update(string? name, int? index)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (index is not null && index != Index) Index = (int)index;
            return this;
        }
    }
}
