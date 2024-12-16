using Microsoft.AspNetCore.Identity;
using ServiceLayer.Constants.Auth;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IdentityRole GetHighestRoleOfUser(IList<string> userRoles)
        {
            if (userRoles.Count == 1) return _roleManager.Roles.First(f => f.Name!.Equals(userRoles.First().ToUpper()));
            else
            {
                string highestRole = string.Empty;

                foreach (string role in UserRoles.Hierarchy)
                {
                    if (userRoles.Contains(role))
                        highestRole = role;
                }

                return _roleManager.Roles.First(f => f.Name!.ToUpper() == highestRole);
            }
        }

        #region Exposing Role Manager methods ONLY through this interface

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        #endregion
    }
}
