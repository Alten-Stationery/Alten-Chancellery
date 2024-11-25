using ServiceLayer.Auth;
using ServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<UserDTO>> CreateUser(UserDTO userDTO, List<string> userRoles);
        Task<Response<bool>> DeleteUser(string id);
        Task<Response<UserDTO>> FindUserById(string id);
        Task<Response<UserDTO>> UpdateUser(UserDTO userDTO);


    }
}
