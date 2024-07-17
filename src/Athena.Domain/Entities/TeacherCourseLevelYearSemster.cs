namespace Athena.Domain.Entities
{
    // State: Active, Finished, Disactive
    public class TeacherCourseLevelYearSemster : AuditableEntity, IAggregateRoot
    {
        public string Semster { get; private set; } = default!;
        public DateTime StartDate { get; private set; } 
        public DateTime EndDate { get; private set; }
        public Guid TeacherCourseLevelYearId { get; private set; }
        public virtual TeacherCourseLevelYear TeacherCourseLevelYear { get; private set; } = default!;

        public TeacherCourseLevelYearSemster(string semster, DateTime startDate, DateTime endDate, Guid teacherCourseLevelYearId, Guid businessId)
        {
            Semster = semster;
            StartDate = startDate;
            EndDate = endDate;
            TeacherCourseLevelYearId = teacherCourseLevelYearId;
            BusinessId = businessId;
        }
        
        public TeacherCourseLevelYearSemster Update(DateTime? startDate, DateTime? endDate)
        {
            if (startDate != null && StartDate != startDate) StartDate = (DateTime)startDate;
            if (endDate != null && EndDate != endDate) EndDate = (DateTime)endDate;
            return this;
        }

    }
}
