namespace Athena.Domain.Entities
{
    public class GroupStudent : AuditableEntity, IAggregateRoot
    {
        public Guid GroupId { get; private set; }
        public virtual Group Group { get; private set; } = default!;

        public Guid TeacherCourseLevelYearStudentId { get; private set; }
        public virtual TeacherCourseLevelYearStudent TeacherCourseLevelYearStudent { get; private set; } = default!;

        public virtual ICollection<ExamGroupStudent> ExamGroupStudents { get; private set; } = default!;

        public GroupStudent(Guid groupId, Guid teacherCourseLevelYearStudentId, Guid businessId) =>
            (GroupId, TeacherCourseLevelYearStudentId, BusinessId) = (groupId, teacherCourseLevelYearStudentId, businessId);

        public GroupStudent Update(Guid? groupId)
        {
            if (groupId.HasValue && groupId.Value != Guid.Empty && !GroupId.Equals(groupId.Value)) GroupId = groupId.Value;
            return this;
        }
        
    }
}
