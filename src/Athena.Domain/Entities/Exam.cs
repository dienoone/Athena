namespace Athena.Domain.Entities
{
    public class Exam : AuditableEntity, IAggregateRoot
    {
        // Models,Total Degree
        public string Name { get; set; } = default!;
        public string? Description { get; private set; }
        public string State { get; private set; }
        public double FinalDegree { get; private set; }
        public int AllowedTime { get; private set; }
        public DateTime PublishedDate { get; private set; }
        public TimeSpan PublishedTime { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsPrime { get; private set; }
        public bool IsReady { get; private set; }
        public Guid TeacherCourseLevelYearId { get; private set; }
        public virtual TeacherCourseLevelYear TeacherCourseLevelYear { get; private set; } = default!;
        public Guid ExamTypeId { get; private set; }
        public virtual ExamType ExamType { get; private set; } = default!;

        public virtual ICollection<Section> Sections { get; private set; } = default!;
        public virtual ICollection<ExamGroup> ExamGroups { get; private set; } = default!;

        public Exam(string name, string? description, string state, double finalDegree, int allowedTime, DateTime publishedDate, TimeSpan publishedTime, bool isActive, bool isPrime, bool isReady, Guid teacherCourseLevelYearId, Guid examTypeId, Guid businessId)
        {
            Name = name;
            Description = description;
            State = state;
            FinalDegree = finalDegree;
            AllowedTime = allowedTime;
            PublishedDate = publishedDate;
            PublishedTime = publishedTime;
            IsActive = isActive;
            IsPrime = isPrime;
            IsReady = isReady;
            TeacherCourseLevelYearId = teacherCourseLevelYearId;
            ExamTypeId = examTypeId;
            BusinessId = businessId;
        }

        public Exam Update(string? name, string? description, string? state, double? finalDegree, int? allowedTime, DateTime? publishedDate,
            TimeSpan? publishedTime, bool? isActive, bool? isPrime, bool? isReady, Guid? examTypeId)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (description is not null && Description?.Equals(description) is not true) Description = description;
            if (state is not null && State?.Equals(state) is not true) State = state;
            if (finalDegree is not null && FinalDegree.Equals(finalDegree) is not true) FinalDegree = (int)finalDegree;
            if (allowedTime is not null && AllowedTime.Equals(allowedTime) is not true) AllowedTime = (int)allowedTime;
            if (publishedDate is not null && PublishedDate.Equals(publishedDate) is not true) PublishedDate = (DateTime)publishedDate;
            if (publishedTime is not null && PublishedTime.Equals(publishedTime) is not true) PublishedTime = (TimeSpan)publishedTime;
            if (isActive is not null && IsActive != isActive) IsActive = (bool)isActive;
            if (isPrime is not null && IsPrime != isPrime) IsPrime = (bool)isPrime;
            if (isReady is not null && IsReady != isReady) IsReady = (bool)isReady;
            if (examTypeId is not null && ExamTypeId.Equals(examTypeId) is not true) ExamTypeId = (Guid)examTypeId;
            return this;
        }
    }
}
