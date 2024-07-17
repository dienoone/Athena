using System.Collections.ObjectModel;

namespace Athena.Shared.Authorization
{
    public static class AAction
    {
        public const string View = nameof(View);
        public const string Base = nameof(Base);
        public const string Search = nameof(Search);
        public const string Create = nameof(Create);
        public const string Update = nameof(Update);
        public const string Toggle = nameof(Toggle);
        public const string Delete = nameof(Delete);
        public const string Take = nameof(Take);
        public const string Export = nameof(Export);
        public const string Generate = nameof(Generate);
        public const string Clean = nameof(Clean);
        public const string UpgradeSubscription = nameof(UpgradeSubscription);
    }

    public static class AResource
    {
        public const string Dashboard = nameof(Dashboard);
        public const string Hangfire = nameof(Hangfire);        
        public const string Users = nameof(Users);
        public const string UserRoles = nameof(UserRoles);
        public const string Roles = nameof(Roles);
        public const string RoleClaims = nameof(RoleClaims);

        public const string Admins = nameof(Admins);


        #region Teacher:

        public const string HeadQuarters = nameof(HeadQuarters);
        public const string Years = nameof(Years);
        public const string Groups = nameof(Groups);
        public const string TeacherStudents = nameof(TeacherStudents);



        #endregion

        #region Dashboard:

        public const string Teachers = nameof(Teachers);
        public const string Courses = nameof(Courses);
        public const string Classifications = nameof(Classifications);
        public const string Levels = nameof(Levels);
        public const string Semsters = nameof(Semsters);
        public const string ExamType = nameof(ExamType);


        #endregion



        // Student:
        public const string Students = nameof(Students);
        public const string StudentsApp = nameof(StudentsApp);
       
    }

    public static class APermissions
    {
        private static readonly APermission[] _all = new APermission[]
        {
            new("View Hangfire", AAction.View, AResource.Hangfire, IsAdmin : false),
            new("View Dashboard", AAction.View, AResource.Dashboard),

            new("View Admins", AAction.View, AResource.Admins),
            new("Search Admins", AAction.Search, AResource.Admins),
            new("Create Admins", AAction.Create, AResource.Admins),
            new("Update Admins", AAction.Update, AResource.Admins),
            new("Delete Admins", AAction.Delete, AResource.Admins),
            new("Export Admins", AAction.Export, AResource.Admins),

            new("View Users", AAction.View, AResource.Users),
            new("Search Users", AAction.Search, AResource.Users),
            new("Create Users", AAction.Create, AResource.Users),
            new("Update Users", AAction.Update, AResource.Users),
            new("Delete Users", AAction.Delete, AResource.Users),
            new("Export Users", AAction.Export, AResource.Users),

            new("View UserRoles", AAction.View, AResource.UserRoles),
            new("Update UserRoles", AAction.Update, AResource.UserRoles),
            new("View Roles", AAction.View, AResource.Roles),
            new("Create Roles", AAction.Create, AResource.Roles),
            new("Update Roles", AAction.Update, AResource.Roles),
            new("Delete Roles", AAction.Delete, AResource.Roles),

            new("View RoleClaims", AAction.View, AResource.RoleClaims),
            new("Update RoleClaims", AAction.Update, AResource.RoleClaims),

            new("Search Teachers", AAction.Search, AResource.Teachers),
            new("View Teachers", AAction.View, AResource.Teachers, IsTeacher: true),
            new("Base Teachers", AAction.Base, AResource.Teachers, IsTeacher: true),
            new("Create Teachers", AAction.Create, AResource.Teachers),
            new("Update Teachers", AAction.Update, AResource.Teachers, IsTeacher: true),
            new("Delete Teachers", AAction.Delete, AResource.Teachers),

            new("Search Classifications", AAction.Search, AResource.Classifications, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("View Classifications", AAction.View, AResource.Classifications, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("Create Classifications", AAction.Create, AResource.Classifications),
            new("Update Classifications", AAction.Update, AResource.Classifications),
            new("Delete Classifications", AAction.Delete, AResource.Classifications),

            new("Search Levels", AAction.Search, AResource.Levels, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("View Levels", AAction.View, AResource.Levels, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("Create Levels", AAction.Create, AResource.Levels),
            new("Update Levels", AAction.Update, AResource.Levels),
            new("Delete Levels", AAction.Delete, AResource.Levels),

            new("Search Students", AAction.Search, AResource.Students, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("View Students", AAction.View, AResource.Students, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("Create Students", AAction.Create, AResource.Students),
            new("Update Students", AAction.Update, AResource.Students),
            new("Delete Students", AAction.Delete, AResource.Students),

            new("Search Courses", AAction.Search, AResource.Courses, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("View Courses", AAction.View, AResource.Courses, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("Create Courses", AAction.Create, AResource.Courses),
            new("Update Courses", AAction.Update, AResource.Courses),
            new("Delete Courses", AAction.Delete, AResource.Courses),

            new("Search HeadQuarters", AAction.Search, AResource.HeadQuarters, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("View HeadQuarters", AAction.View, AResource.HeadQuarters, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("Create HeadQuarters", AAction.Create, AResource.HeadQuarters, IsTeacher: true),
            new("Update HeadQuarters", AAction.Update, AResource.HeadQuarters, IsTeacher: true),
            new("Delete HeadQuarters", AAction.Delete, AResource.HeadQuarters, IsTeacher: true),

            new("Search Years", AAction.Search, AResource.Years, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("View Years", AAction.View, AResource.Years, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("Create Years", AAction.Create, AResource.Years, IsTeacher: true),
            new("Toggle Years", AAction.Toggle, AResource.Years, IsTeacher: true),
            new("Update Years", AAction.Update, AResource.Years, IsTeacher: true),
            new("Delete Years", AAction.Delete, AResource.Years, IsTeacher: true),

            new("Search Semsters", AAction.Search, AResource.Semsters, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("View Semsters", AAction.View, AResource.Semsters, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("Create Semsters", AAction.Create, AResource.Semsters),
            new("Update Semsters", AAction.Update, AResource.Semsters),
            new("Delete Semsters", AAction.Delete, AResource.Semsters),

            new("Search ExamTypes", AAction.Search, AResource.ExamType, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("View ExamTypes", AAction.View, AResource.ExamType, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("Create ExamTypes", AAction.Create, AResource.ExamType),
            new("Update ExamTypes", AAction.Update, AResource.ExamType),
            new("Delete ExamTypes", AAction.Delete, AResource.ExamType),

            new("Search Groups", AAction.Search, AResource.Groups, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("View Groups", AAction.View, AResource.Groups, IsTeacher: true, IsEmployee: true, IsStudent: true),
            new("Create Groups", AAction.Create, AResource.Groups, IsTeacher: true, IsEmployee: true),
            new("Update Groups", AAction.Update, AResource.Groups, IsTeacher: true, IsEmployee: true),
            new("Delete Groups", AAction.Delete, AResource.Groups, IsTeacher: true, IsEmployee: true),


            new("View StudentsApp", AAction.View, AResource.StudentsApp, IsSuperAdmin: false, IsAdmin: false, IsTeacher: false ,IsEmployee: false, IsStudent: true),

            /*new("View Tenants", AAction.View, AResource.Tenants, IsRoot: true),
            new("Create Tenants", AAction.Create, AResource.Tenants, IsRoot: true),
            new("Update Tenants", AAction.Update, AResource.Tenants, IsRoot: true),
            new("Upgrade Tenant Subscription", AAction.UpgradeSubscription, AResource.Tenants, IsRoot: true)*/
        };

        public static IReadOnlyList<APermission> All { get; } = new ReadOnlyCollection<APermission>(_all);
        public static IReadOnlyList<APermission> Admin { get; } = new ReadOnlyCollection<APermission>(_all.Where(p => p.IsAdmin).ToArray());
        public static IReadOnlyList<APermission> Teacher { get; } = new ReadOnlyCollection<APermission>(_all.Where(p => p.IsTeacher).ToArray());
        public static IReadOnlyList<APermission> Employee { get; } = new ReadOnlyCollection<APermission>(_all.Where(p => p.IsEmployee).ToArray());
        public static IReadOnlyList<APermission> Student { get; } = new ReadOnlyCollection<APermission>(_all.Where(p => p.IsStudent).ToArray());
        
    }

    public record APermission(string Description, string Action, string Resource,
        bool IsSuperAdmin = true, bool IsAdmin = true, bool IsTeacher = false, bool IsEmployee = false, bool IsStudent = false)
    {
        public string Name => NameFor(Action, Resource);
        public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
    }
}
