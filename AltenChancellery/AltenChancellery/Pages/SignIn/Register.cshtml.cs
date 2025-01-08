using DBLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;
using System.Net;

namespace AltenChancellery.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        private readonly IUserService _userService;

        public RegisterModel(SignInManager<User> signInManager, IUserService userService)
        {
            _signInManager = signInManager;
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string name, string surname, string email, string password)
        {
            bool existCheck = await _signInManager.UserManager.FindByEmailAsync(email) != null;

            if (existCheck)
                return Redirect("/SignIn/Login"); // Email already registered

            UserDTO newUser = new UserDTO()
            {
                Name = name,
                Surname = surname,
                Email = email,
                Password = password
            };

            Response<UserDTO> result = await _userService.CreateUserAsync(newUser);

            if (result.StatusCode != HttpStatusCode.OK
                && result.StatusCode != HttpStatusCode.Created)
                return Redirect("/Error"); // Temporary error handler

            User? user = await _signInManager.UserManager.FindByEmailAsync(email);

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user!, password, true, false);

            if(!signInResult.Succeeded)
                return Redirect("/Error"); // TODO: redirect with error
            
            return Redirect("/");
        }
    }
}
