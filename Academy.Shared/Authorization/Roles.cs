using System.Collections.ObjectModel;

namespace Academy.Shared.Authorization
{
    public static class Roles
    {
        public const string SAdmin = nameof(SAdmin);
        public const string Owner = nameof(Owner);
        public const string Admin = nameof(Admin);
        //public const string Member = nameof(Member);

        public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
        {
            SAdmin
        });

        public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
    }
}