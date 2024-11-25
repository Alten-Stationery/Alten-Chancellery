using DBLayer.Models;
using DBLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task<bool> AddRole(User user, string role)
        {
            try
            {
                var res = await _userManager.AddToRoleAsync(user, role);
                return res.Succeeded;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<User?> AddUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");

            try
            {
                var userExist = await _userManager.FindByEmailAsync(user.Email);
                if (userExist != null)
                {
                    return null;
                }
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    return user;
                }
                throw new InvalidOperationException("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            catch (Exception)
            {
                throw;
            }


        }

        public Task<bool> DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindUser(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
