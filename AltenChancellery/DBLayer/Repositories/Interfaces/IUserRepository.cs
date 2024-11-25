using DBLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> AddUser(User user);
        Task<bool> AddRole(User user, String role);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
        Task<User> FindUser(string Id);
        Task<User> FindUserByEmail(string email);
        
    }
}
