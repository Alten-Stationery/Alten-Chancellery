using AutoMapper;
using DBLayer.Models;
using DBLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Auth;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserService(IUserRepository userRepository, RoleManager<IdentityRole> roleManager, IMapper mapper, UserManager<User> userManager) 
        { 
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;          
        }
        public async Task<Response<UserDTO>> CreateUser(UserDTO userDTO)
        {
            try
            {
                List<string> roles = new List<string>() { UserRoles.User };
                var response = await CreateUser(userDTO, roles);
                return response;
            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = $"Error: {ex.Message}" };
            }
        }
        public async Task<Response<UserDTO>> CreateRLS(UserDTO userDTO)
        {
            try
            {
                List<string> roles = new List<string>() { UserRoles.User, UserRoles.Rls };
                var response = await CreateUser(userDTO, roles);
                return response;
            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = $"Error: {ex.Message}" };
            }
        }
        
        public async Task<Response<UserDTO>> CreateAdmin(UserDTO userDTO)
        {
            try 
            {
                List<string> roles = new List<string>() { UserRoles.User, UserRoles.Rls, UserRoles.Admin };
                var response = await CreateUser(userDTO, roles);
                return response;
            }
            catch(Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = $"Error: {ex.Message}" };
            }
        }
        
        private async Task<Response<UserDTO>> CreateUser(UserDTO userDTO, List<string> userRoles)
        {
            try
            { 
                var user = _mapper.Map<User>(userDTO);
                if (userDTO == null) throw new ArgumentNullException(nameof(userDTO), "User cannot be null");
                if (await _userRepository.UserExist(userDTO.Email)) throw new Exception("User Already exist");
                var response = await _userRepository.AddUser(user);
                
                if (response == null) return new Response<UserDTO> { Message = "User not Saved", StatusCode = System.Net.HttpStatusCode.InternalServerError };

                foreach (var role in userRoles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                        await _userRepository.AddRole(response, role);
                }
                var userDtoToSend = _mapper.Map<UserDTO>(response);
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = userDtoToSend };
            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<Response<bool>> DeleteUser(string id)
        {
            try
            {
                var userToDelete = await _userRepository.FindUserById(id);
                var res = await _userRepository.DeleteUser(userToDelete);
                if(res) return new Response<bool> {StatusCode = System.Net.HttpStatusCode.OK,  Data=res};
                return new Response<bool> { Data = res, StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error: User not Removed" };
            }
            catch (Exception ex)
            {
                return new Response<bool> { Message = ex.Message, StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        public async Task<Response<UserDTO>> FindUserById(string id)
        {
            try
            {
                var user = await _userRepository.FindUserById(id);
                if (user is null) return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "User not Found" };
                var userToSend = _mapper.Map<UserDTO>(user);
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = userToSend };

            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { Message = ex.Message, StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        public async Task<Response<UserDTO>> UpdateUser(UserDTO userDTO)
        {
            try
            {
                if (userDTO == null) throw new ArgumentNullException(nameof(userDTO), "User cannot be null");  
                var userToUpdate = _mapper.Map<User>(userDTO);
                var res = await _userRepository.UpdateUser(userToUpdate);
                if (res is null) return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Error during User Update" };
                var userToSend = _mapper.Map<UserDTO>(res);
                return new Response<UserDTO> {StatusCode = System.Net.HttpStatusCode.OK,Data = userToSend};
            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }
    }
}
