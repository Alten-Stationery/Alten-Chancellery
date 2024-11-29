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
        public async Task<bool> UserExist(string email) 
        {
            try
            {
                if(await _userManager.FindByEmailAsync(email) == null) return false;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<User?> AddUser(User user)
        {
            

            try
            {
                
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

        public async Task<bool> DeleteUser(User user)
        {
            try
            {
                var res = await _userManager.DeleteAsync(user);
                return res.Succeeded;
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> FindUserById(string id)
        {
            try
            {
                var res = await _userManager.FindByIdAsync(id);
                return res;
            }
            catch
            {
                throw; 
            }
        }

        public async Task<User> FindUserByEmail(string email)
        {
            try
            {
                var res = await _userManager.FindByEmailAsync(email);
                return res;
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
                var res = await _userManager.UpdateAsync(user);
                return user;
            }
            catch 
            {
                throw;
            }
        }
    }
}
