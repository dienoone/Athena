namespace Athena.Domain.Entities
{
    public class Student : BaseEntity, IAggregateRoot
    {
        // Personal Info
        public string Name { get; private set; } = default!;
        public string Gender { get; private set; } = default!;
        public string Code { get; private set; } = default!;
        public string? Address { get; private set; }
        public string? Image { get; private set; }
        public DateTime? BirthDay { get; private set; }

        // Parent Info
        public string ParentName { get; private set; } = default!;
        public string ParentJob { get; private set; } = default!;
        public string ParentPhone { get; private set; } = default!;
        public string HomePhone { get; private set; } = default!;

        // Education Level Info
        public string School { get; private set; } = default!;
        public Guid LevelClassificationId { get; private set; }

        // Relations:
        public virtual LevelClassification LevelClassification { get; private set; } = default!;
        public virtual ICollection<TeacherCourseLevelYearStudent> TeacherCourseLevelYearStudents { get; private set; } = default!;
        public virtual ICollection<StudentTeacherRequest> StudentTeacherRequests { get; private set; } = default!;
        public virtual ICollection<StudentTeacherCommunication> StudentTeacherCommunications { get; private set; } = default!;


        public Student(Guid id, string name, string gender, string? address, string? image, DateTime? birthDay,
            string parentName, string parentJob, string parentPhone, string homePhone,
            string school, string code, Guid levelClassificationId)
        {
            Id = id;
            Name = name;
            Gender = gender;
            Address = address;
            Image = image;
            BirthDay = birthDay;
            ParentName = parentName;
            ParentJob = parentJob;
            ParentPhone = parentPhone;
            HomePhone = homePhone;
            School = school;
            Code = code;
            LevelClassificationId = levelClassificationId;
        }

        public Student Update(string? name, string? gender, string? address, string? image, DateTime? birthDay, 
            string? parentName, string? parentJob, string? parentPhone, string? homePhone,
            string? school, Guid? levelClassificationId)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (gender is not null && Gender?.Equals(gender) is not true) Gender = gender;
            if (address is not null && Address?.Equals(address) is not true) Address = address;
            if (image is not null && Image?.Equals(image) is not true) Image = image;
            if (birthDay is not null && BirthDay?.Equals(birthDay) is not true) BirthDay = birthDay;

            if (parentName is not null && ParentName?.Equals(parentName) is not true) ParentName = parentName;
            if (parentJob is not null && ParentJob?.Equals(parentJob) is not true) ParentJob = parentJob;
            if (parentPhone is not null && ParentPhone?.Equals(parentPhone) is not true) ParentPhone = parentPhone;
            if (homePhone is not null && HomePhone?.Equals(homePhone) is not true) HomePhone = homePhone;

            if (school is not null && School?.Equals(school) is not true) School = school;
            if (levelClassificationId.HasValue && levelClassificationId.Value != Guid.Empty && !LevelClassificationId.Equals(levelClassificationId.Value)) LevelClassificationId = levelClassificationId.Value;
            return this;
        }

        public Student ClearImagePath()
        {
            Image = string.Empty;
            return this;
        }

    }
}
