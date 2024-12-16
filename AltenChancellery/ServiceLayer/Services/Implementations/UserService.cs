using AutoMapper;
using DBLayer.DBContext;
using DBLayer.Models;
using DBLayer.Repositories.Interfaces;
using DBLayer.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Constants.Auth;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        private readonly IRoleService _roleService;

        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository, IRoleService roleService, IMapper mapper, UserManager<User> userManager, IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;

            _userManager = userManager;
            _roleService = roleService;
        }

        public async Task<Response<UserDTO>> CreateUserAsync(UserDTO userDTO)
        {
            try
            {
                List<string> roles = new List<string>() { UserRoles.User };
                var response = await CreateAsync(userDTO, roles);

                return response;
            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<Response<UserDTO>> CreateRLSAsync(UserDTO userDTO)
        {
            try
            {
                List<string> roles = new List<string>() { UserRoles.User, UserRoles.Rls };
                var response = await CreateAsync(userDTO, roles);

                return response;
            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = $"Error: {ex.Message}" };
            }
        }
        
        public async Task<Response<UserDTO>> CreateAdminAsync(UserDTO userDTO)
        {
            try 
            {
                List<string> roles = new List<string>() { UserRoles.User, UserRoles.Rls, UserRoles.Admin };
                var response = await CreateAsync(userDTO, roles);
                return response;
            }
            catch(Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = $"Error: {ex.Message}" };
            }
        }
        
        private async Task<Response<UserDTO>> CreateAsync(UserDTO userDTO, List<string> userRoles)
        {
            try
            { 
                var user = _mapper.Map<User>(userDTO);
                if (userDTO == null) throw new ArgumentNullException(nameof(userDTO), "User cannot be null");
                if (await _uow.UserRepo.UserExist(userDTO.Email)) throw new Exception("User Already exist");
                var response = await _uow.UserRepo.AddUser(user);
                
                if (response == null) return new Response<UserDTO> { Message = "User not Saved", StatusCode = System.Net.HttpStatusCode.InternalServerError };

                await _uow.SaveAsync();

                foreach (var role in userRoles)
                {
                    if (await _roleService.RoleExistsAsync(role))
                        await _uow.UserRepo.AddRole(response, role);
                }
                var userDtoToSend = _mapper.Map<UserDTO>(response);

                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = userDtoToSend };
            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<Response<bool>> DeleteUserAsync(string id)
        {
            try
            {
                var userToDelete = await _uow.UserRepo.FindUserById(id);
                var res = await _uow.UserRepo.DeleteUser(userToDelete);

                if (res)
                {
                    await _uow.SaveAsync();
                    return new Response<bool> {StatusCode = System.Net.HttpStatusCode.OK,  Data=res};
                }

                return new Response<bool> { Data = res, StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = "Error: User not Removed" };
            }
            catch (Exception ex)
            {
                return new Response<bool> { Message = ex.Message, StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        public async Task<Response<UserDTO>> FindUserByIdAsync(string id)
        {
            try
            {
                var user = await _uow.UserRepo.FindUserById(id);
                if (user is null) return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "User not Found" };
                var userToSend = _mapper.Map<UserDTO>(user);
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.OK, Data = userToSend };

            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { Message = ex.Message, StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        public async Task<Response<UserDTO>> UpdateUserAsync(UserDTO userDTO)
        {
            try
            {
                if (userDTO == null) throw new ArgumentNullException(nameof(userDTO), "User cannot be null");  
                var userToUpdate = _mapper.Map<User>(userDTO);
                var res = await _uow.UserRepo.UpdateUser(userToUpdate);
                if (res is null) return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Error during User Update" };
                var userToSend = _mapper.Map<UserDTO>(res);
                return new Response<UserDTO> {StatusCode = System.Net.HttpStatusCode.OK,Data = userToSend};
            }
            catch (Exception ex)
            {
                return new Response<UserDTO> { StatusCode = System.Net.HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        #region Exposing UserManager methods

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        #endregion
    }
}
