namespace Athena.Domain.Entities
{
    // BusinessId: TeacherID => UserId
    public class Teacher : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; } = default!;
        public string Gender { get; private set; } = default!;
        public string? Address { get; private set; }
        public string? ImagePath { get; private set; }
        public DateTime? BirthDay { get; private set; }
        public string? CoverImagePath { get; private set; }
        public string? Summary { get; private set; }
        public string? Nationality { get; set; }
        public string? Degree { get; private set; }
        public string? School { get; private set; }
        public string? TeachingMethod { get; private set; }
        public Guid CourseId { get; private set; }
        public virtual Course Course { get; private set; } = null!;

        public virtual ICollection<TeacherContact> TeacherContacts { get; private set; } = default!;
        public virtual ICollection<TeacherCourseLevel> TeacherCourseLevels { get; private set; } = default!;
        public virtual ICollection<HeadQuarter> HeadQuarters { get; private set; } = default!;
        public virtual ICollection<StudentTeacherRequest> StudentTeacherRequests { get; private set; } = default!;
        public virtual ICollection<StudentTeacherCommunication> StudentTeacherCommunications { get; private set; } = default!;

        public Teacher(Guid id, string name, string gender, string? address, string? imagePath, DateTime? birthDay, string? coverImagePath, string? summary, string? nationality,
            string? degree, string? school, string? teachingMethod, Guid courseId, Guid businessId)
        {
            Id = id;
            Name = name;
            Gender = gender;
            Address = address;
            ImagePath = imagePath;
            BirthDay = birthDay;
            CoverImagePath = coverImagePath;
            Summary = summary;
            Nationality = nationality;
            Degree = degree;
            School = school;
            TeachingMethod = teachingMethod;
            CourseId = courseId;
            BusinessId = businessId;
        }

        public Teacher Update(string? name, string? gender, string? address, string? imagePath, DateTime? birthDay, string? coverImagePath, string? summary, string? nationality,
            string? degree, string? school, string? teachingMethod, Guid? courseId)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (gender is not null && Gender?.Equals(gender) is not true) Gender = gender;
            if (address is not null && Address?.Equals(address) is not true) Address = address;
            if (imagePath is not null && ImagePath?.Equals(imagePath) is not true) ImagePath = imagePath;
            if (birthDay is not null && BirthDay.Equals(birthDay) is not true) BirthDay = (DateTime)birthDay;
            if (coverImagePath is not null && CoverImagePath?.Equals(coverImagePath) is not true) CoverImagePath = coverImagePath;
            if (summary is not null && Summary?.Equals(summary) is not true) Summary = summary;
            if (nationality is not null && Nationality?.Equals(nationality) is not true) Nationality = nationality;
            if (degree is not null && Degree?.Equals(degree) is not true) Degree = degree;
            if (school is not null && School?.Equals(school) is not true) School = school;
            if (teachingMethod is not null && TeachingMethod?.Equals(teachingMethod) is not true) TeachingMethod = teachingMethod;
            if (courseId.HasValue && courseId.Value != Guid.Empty && !CourseId.Equals(courseId.Value)) CourseId = courseId.Value;
            return this;
        }

        public Teacher ClearImagePath()
        {
            ImagePath = string.Empty;
            return this;
        }

        public Teacher ClearCoverImagePath()
        {
            CoverImagePath = string.Empty;
            return this;
        }
    }
}
