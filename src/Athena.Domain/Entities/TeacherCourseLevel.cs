namespace Athena.Domain.Entities
{
    // BuesinessId: TeacherID
    public class TeacherCourseLevel : AuditableEntity, IAggregateRoot
    {
        public Guid TeacherId { get; private set; }
        public virtual Teacher Teacher { get; private set; } = default!;

        public Guid LevelId { get; private set; }
        public virtual Level Level { get; private set; } = default!;

        public virtual ICollection<TeacherCourseLevelYear> TeacherCourseLevelYears { get; private set; } = default!;

        /* public virtual ICollection<TeacherCourseLevelHeadQuarter> TeacherCourseHeadQuarters { get; private set; } = default!;
         public virtual ICollection<TeacherCourseLevelStudent> TeacherCourseLevelStudents { get; private set; } = default!;
         public virtual ICollection<TeacherCourseLevelSemster> TeacherCourseLevelSemsters { get; private set; } = default!;*/

        public TeacherCourseLevel(Guid teacherId, Guid levelId, Guid businessId)
        {
            TeacherId = teacherId;
            LevelId = levelId;
            BusinessId = businessId;
        }

        public TeacherCourseLevel Update(Guid? levelId)
        {
            if (levelId.HasValue && levelId.Value != Guid.Empty && !LevelId.Equals(levelId.Value)) LevelId = levelId.Value;
            return this;
        }


    }
}
