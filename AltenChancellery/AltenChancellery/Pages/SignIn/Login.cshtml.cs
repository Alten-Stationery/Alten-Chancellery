using DBLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Constants.Auth;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        private readonly ITokenService _tokenService;

        public LoginModel(ITokenService tokenService, SignInManager<User> signInManager)
        {
            _tokenService = tokenService;

            _signInManager = signInManager;
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            User? currentUser = await _signInManager.UserManager.FindByEmailAsync(email);

            if (currentUser is not null && await _signInManager.UserManager.CheckPasswordAsync(currentUser, password))
            {
                // generates jwt and refresh tokens then saves refresh token to DB
                bool saveResult = _tokenService.GenerateAndSaveTokens(currentUser, out string accessToken, out string refreshToken);

                if (!saveResult)
                    return Redirect("/Error");
                
                // save tokens in cookies & in header
                UpdateTokensInHeader(accessToken, refreshToken);
                UpdateTokensInCookies(accessToken, refreshToken);

                await _signInManager.SignInAsync(currentUser, true);

                return Redirect("/");
            }
            else return Page(); // wrong password
        }

        private void UpdateTokensInHeader(string accessToken, string refreshToken)
        {
            HttpContext.Response.Headers.Append(TokenConst.AccessToken, accessToken);
            HttpContext.Response.Headers.Append(TokenConst.RefreshToken, refreshToken);
        }

        private void UpdateTokensInCookies(string accessToken, string refreshToken)
        {
            // Store jwt token inside cookies too
            HttpContext.Response.Cookies.Append(TokenConst.AccessToken, accessToken, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax, Secure = true });
            HttpContext.Response.Cookies.Append(TokenConst.RefreshToken, refreshToken, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax, Secure = true });
        }
    }
}
