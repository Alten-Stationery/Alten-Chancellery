using Azure.Core;
using DBLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Constants.Auth;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Pages.SignIn
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();

            RemoveTokensFromCookies();

            return Redirect("/SignIn/Login");
        }

        private void RemoveTokensFromCookies()
        {
            HttpContext.Response.Cookies.Delete(TokenConst.AccessToken);
            HttpContext.Response.Cookies.Delete(TokenConst.RefreshToken);
        }
    }
}
