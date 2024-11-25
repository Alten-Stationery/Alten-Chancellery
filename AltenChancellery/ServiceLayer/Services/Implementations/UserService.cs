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
        public UserService(IUserRepository userRepository, RoleManager<IdentityRole> roleManager, IMapper mapper) 
        { 
            _userRepository = userRepository;
            _roleManager = roleManager;
            _mapper = mapper;          
        }

        public async Task<Response<UserDTO>> CreateUser(UserDTO userDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userDTO);
                var response = await _userRepository.AddUser(user);
                
                if (response == null) return new Response<UserDTO> { Message = "User not Saved", StatusCode = System.Net.HttpStatusCode.InternalServerError };
                if (await _roleManager.RoleExistsAsync(UserRoles.User))
                    await _userRepository.AddRole(response, UserRoles.User);

                var userDtoToSend = _mapper.Map<UserDTO>(response);
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = userDtoToSend };
            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = $"Error: {ex.Message}" };
            }
        }
    }
}
