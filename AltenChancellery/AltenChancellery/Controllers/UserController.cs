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
                var res = await _userService.CreateUser(user);
                return Ok(res);
            }
            catch(Exception ex) 
            {
               return StatusCode(500, ex.Message);
            }
            
        }
    }
}
