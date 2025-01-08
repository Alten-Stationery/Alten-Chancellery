using DBLayer.Models;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<UserDTO>> CreateUserAsync(UserDTO userDTO);
        Task<Response<UserDTO>> CreateRLSAsync(UserDTO userDTO);
        Task<Response<UserDTO>> CreateAdminAsync(UserDTO userDTO);
        Task<Response<bool>> DeleteUserAsync(string id);
        Task<Response<UserDTO>> FindUserByIdAsync(string id);
        Task<Response<UserDTO>> UpdateUserAsync(UserDTO userDTO);

        // this region also exists in this interface's implementation, please keep there when adding new methods
        #region Exposing UserManager methods ONLY through this interface

        Task<IList<string>> GetRolesAsync(User user);
        

        #endregion

    }
}
