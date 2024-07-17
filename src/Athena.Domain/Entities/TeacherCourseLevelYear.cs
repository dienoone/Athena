namespace Athena.Domain.Entities
{
    public class TeacherCourseLevelYear : AuditableEntity, IAggregateRoot
    {
        public Guid TeacherCourseLevelId { get; private set; }
        public TeacherCourseLevel TeacherCourseLevel { get; private set; } = default!;

        public Guid YearId { get; private set; }
        public Year Year { get; private set; } = default!;

        public int IntroFee { get; private set; }
        public int MonthFee { get; private set; }

        public virtual ICollection<TeacherCourseLevelYearStudent> TeacherCourseLevelYearStudents { get; private set; } = default!;
        public virtual ICollection<TeacherCourseLevelYearSemster> TeacherCourseLevelYearSemsters { get; private set; } = default!;
        public virtual ICollection<Group> Groups { get; private set; } = default!;
        public virtual ICollection<Exam> Exams { get; private set; } = default!;

        public virtual ICollection<Attendence> Attendences { get; private set; } = default!;

        public TeacherCourseLevelYear(Guid teacherCourseLevelId, Guid yearId, int introFee, int monthFee, Guid businessId)
        {
            TeacherCourseLevelId = teacherCourseLevelId;
            YearId = yearId;
            IntroFee = introFee;
            MonthFee = monthFee;
            BusinessId = businessId;
        }

        public TeacherCourseLevelYear Update(int? introFee, int? monthFee)
        {
            if (introFee is not null && IntroFee.Equals(introFee) is not true) IntroFee = (int)introFee;
            if (monthFee is not null && MonthFee.Equals(monthFee) is not true) MonthFee = (int)monthFee;
            return this;
        }

        
    }
}
