﻿using AltenChancellery.Auth;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userservice) 
        {
            _userService = userservice;
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO user)
        {
            try 
            {
                List<string> userRoles = new List<string> { UserRoles.User };
                var res = await _userService.CreateUser(user, userRoles);
                return Ok(res);
            }
            catch(Exception ex) 
            {
               return StatusCode(500, ex.Message);
            }
            
        }
        [HttpGet]
        [Route("FindUserById")]
        public async Task<IActionResult> FindUserById(string id) 
        {
            try
            {
                var res = await _userService.FindUserById(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteUserById")]
        public async Task<IActionResult> DeleteUserByID(string id)        
        {
            try
            {
                var res = await _userService.DeleteUser(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDTO user)
        {
            try
            {
                var res = await _userService.UpdateUser(user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
