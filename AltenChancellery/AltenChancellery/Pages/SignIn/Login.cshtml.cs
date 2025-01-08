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

        private readonly IRoleService _roleService;
        private readonly ITokenService _tokenService;

        public LoginModel(ITokenService tokenService, SignInManager<User> signInManager, IRoleService roleService)
        {
            _tokenService = tokenService;

            _signInManager = signInManager;
            _roleService = roleService;
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            User? currentUser = await _signInManager.UserManager.FindByEmailAsync(email);

            if (currentUser is not null && await _signInManager.UserManager.CheckPasswordAsync(currentUser, password))
            {
                IList<string> roles = await _signInManager.UserManager.GetRolesAsync(currentUser);

                IdentityRole role = _roleService.GetHighestRoleOfUser(roles);

                // generate tokens
                string accessToken = _tokenService.GenerateJwtAccessToken(currentUser, role);
                RefreshToken refreshToken = _tokenService.GenerateRefreshToken(currentUser);
                //

                // save refresh token to DB
                bool saveResult = _tokenService.SaveRefreshToken(refreshToken);

                if (!saveResult)
                    return Redirect("/Error");
                
                // save tokens in cookies & in header
                UpdateTokensInHeader(accessToken, refreshToken.Token);
                UpdateTokensInCookies(accessToken, refreshToken.Token!);

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
