using System.Collections.Immutable;

namespace ServiceLayer.Constants.Auth
{
    public class UserRoles
    {
        public const string Admin = "ADMIN";
        public const string User = "USER";
        public const string Rls = "RLS";

        // Keep hierarchy of roles from least to most authority
        public static ImmutableArray<string> Hierarchy = ImmutableArray.CreateRange(
            [
            // Low authority ↓
                User,
                Rls,
                Admin,
            // High authority ↑
            ]
            );
    }
}
