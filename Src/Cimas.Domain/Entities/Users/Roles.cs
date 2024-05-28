namespace Cimas.Domain.Entities.Users
{
    public static class Roles
    {
        public const string Owner = "Owner";
        public const string Worker = "Worker";
        public const string Reviewer = "Reviewer";

        public static string[] GetRoles()
            => [Owner, Worker, Reviewer];

        public static string[] GetNonOwnerRoles()
            => [Worker, Reviewer];

        public static bool IsRoleValid(this string role)
            => GetRoles().Contains(role);

        public static bool IsOwner(this string role)
            => role == Owner;

        public static bool IsNonOwner(this string role)
            => GetNonOwnerRoles().Contains(role);
    }
}
