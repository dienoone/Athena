namespace Athena.Domain.Entities
{
    public class Group : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; } = default!;
        public Guid HeadQuarterId { get; private set; }
        public virtual HeadQuarter HeadQuarter { get; set; } = default!;
        public Guid TeacherCourseLevelYearId { get; private set; }
        public virtual TeacherCourseLevelYear TeacherCourseLevelYear { get; private set; } = default!;
        public int Limit { get; private set; }

        public virtual ICollection<GroupScadual> GroupScaduals { get; private set; } = default!;
        public virtual ICollection<GroupStudent> GroupStudents { get; private set; } = default!;
        public virtual ICollection<ExamGroup> ExamGroups { get; private set; } = default!;
        public virtual ICollection<StudentTeacherRequest> StudentTeacherRequests { get; private set; } = default!;

        public Group(string name, Guid headQuarterId, Guid teacherCourseLevelYearId, int limit, Guid businessId)
        {
            Name= name;
            HeadQuarterId= headQuarterId;
            TeacherCourseLevelYearId= teacherCourseLevelYearId;
            Limit= limit;
            BusinessId= businessId;
        }

        public Group Update(string? name, Guid? headQuarterId, Guid? teacherCourseLevelYearId, int? limit)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (headQuarterId.HasValue && headQuarterId.Value != Guid.Empty && !HeadQuarterId.Equals(headQuarterId.Value)) HeadQuarterId = headQuarterId.Value;
            if (teacherCourseLevelYearId.HasValue && teacherCourseLevelYearId.Value != Guid.Empty && !TeacherCourseLevelYearId.Equals(teacherCourseLevelYearId.Value)) TeacherCourseLevelYearId = teacherCourseLevelYearId.Value;
            if (limit != null && Limit != limit) Limit = (int)limit;
            return this;
        }

    }
}
