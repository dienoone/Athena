using System.Collections.ObjectModel;

namespace Athena.Shared.Authorization
{
    public static class ARoles
    {
        public const string SuperAdmin = nameof(SuperAdmin);
        public const string Admin = nameof(Admin);
        public const string Teacher = nameof(Teacher);
        public const string Employee = nameof(Employee);
        public const string Student = nameof(Student);

       
        public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
        {
            SuperAdmin,
            Admin,
            Teacher,
            Employee,
            Student,
        });

        public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
    }
}
