using Microsoft.AspNetCore.Identity;

namespace ServiceLayer.Services.Interfaces
{
    public interface IRoleService
    {
        public IdentityRole GetHighestRoleOfUser(IList<string> userRoles);
    }
}
