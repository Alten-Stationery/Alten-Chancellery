using Microsoft.AspNetCore.Identity;

namespace ServiceLayer.Services.Interfaces
{
    public interface IRoleService
    {
        public IdentityRole GetHighestRoleOfUser(IList<string> userRoles);

        // this region also exists in this interface's implementation, please keep there when adding new methods
        #region Exposing Role Manager methods ONLY through this interface

        Task<bool> RoleExistsAsync(string roleName);

        #endregion
    }
}
