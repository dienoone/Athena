using Athena.Infrastructure.Persistence.Configuration;

namespace Athena.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IEventPublisher events)
            : base(options, currentUser, serializer, events)
        {
        }

        #region Basics:

        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<EducationClassification> Classifications { get; set; } = null!;
        public DbSet<Level> Levels { get; set; } = null!;
        public DbSet<LevelClassification> LevelClassifications { get; set; } = null!;
        public DbSet<DashboardYear> DashboardYears { get; set; } = null!;

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<TeacherContact> TeacherContacts { get; set; } = null!;
        public DbSet<TeacherCourseLevel> TeacherCourseLevels { get; set; } = null!;

        public DbSet<HeadQuarter> HeadQuarters { get; set; } = null!;
        public DbSet<HeadQuarterPhone> HeadQuarterPhones { get; set; } = null!;

        public DbSet<Year> Years { get; set; } = null!;
        public DbSet<TeacherCourseLevelYear> TeacherCourseLevelYears { get; set; } = null!;
        public DbSet<TeacherCourseLevelYearSemster> TeacherCourseLevelYearSemsters { get; set; } = null!;

        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<GroupScadual> GroupScaduals { get; set; } = null!;

        public DbSet<TeacherCourseLevelYearStudent> TeacherCourseLevelYearStudents { get; set; } = null!;
        public DbSet<GroupStudent> GroupStudents { get; set; } = null!;

        public DbSet<ExamType> ExamTypes { get; set; } = null!;
        public DbSet<Exam> Exams { get; set; } = null!;
        public DbSet<Section> Sections { get; set; } = null!;
        public DbSet<SectionImage> SectionImages { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionImage> QuestionImages { get; set; } = null!;
        public DbSet<QuestionChoice> QuestionChoices { get; set; } = null!;

        public DbSet<ExamGroup> ExamGroups { get; set; } = null!;
        public DbSet<ExamGroupStudent> ExamGroupStudents { get; set; } = null!;
        public DbSet<ExamStudentAnswer> ExamStudentAnswers { get; set; } = null!;


        public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; } = null!;
        public DbSet<CodeHelper> CodeHelpers { get; set; } = null!;




        #region Notifications:

        public DbSet<NotificationType> NotificationTypes { get; set; } = null!;
        public DbSet<NotificationTypeTemplate> NotificationTypeTemplates { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<NotificationMessage> NotificationMessages { get; set; } = null!;
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; } = null!;

        #endregion

        #region Requests:

        public DbSet<StudentTeacherRequest> StudentTeacherRequests { get; set; } = null!;
        public DbSet<StudentTeacherCommunication> StudentTeacherCommunications { get; set; } = null!;

        #endregion

        #region SignalRConnections:

        public DbSet<SignalRConnection> SignalRConnections { get; set; } = null!;
        public DbSet<SignalRConnectionGroup> signalRConnectionGroups { get; set; } = null!;
        public DbSet<StudentSectionState> StudentSectionStates { get; set; } = null!;

        #endregion


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
        }
    }
}
