namespace Athena.Domain.Entities
{
    public class TeacherCourseLevelYearStudent : AuditableEntity, IAggregateRoot
    {
        public Guid TeacherCourseLevelYearId { get; private set; }
        public virtual TeacherCourseLevelYear TeacherCourseLevelYear { get; private set; } = default!;

        public Guid StudentId { get; private set; }
        public virtual Student Student { get; private set; } = default!;

        public int IntroFee { get; set; }

        public virtual ICollection<GroupStudent> GroupStudents { get; private set; } = default!;

        public TeacherCourseLevelYearStudent(Guid teacherCourseLevelYearId, Guid studentId, int introFee, Guid businessId) =>
            (TeacherCourseLevelYearId, StudentId, IntroFee, BusinessId) = (teacherCourseLevelYearId, studentId, introFee, businessId);

        public TeacherCourseLevelYearStudent Update(int? introFee)
        {
            if (introFee is not null && IntroFee.Equals(introFee) is not true) IntroFee = (int)introFee;
            return this;
        }
       
    }
}
