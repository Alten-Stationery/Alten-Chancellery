using DBLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Constants.Auth;
using ServiceLayer.DTOs;
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

        public void OnGet() { }

        [NonHandler]
        public async Task OnGetTokenCheck(RefreshToken storedRefreshToken)
        {
            if (storedRefreshToken.ExpirationDate < DateTime.UtcNow)
                Redirect("/Login"); // Refresh token expired, need new one from login process

            Tokens tokens = await _tokenService.RefreshToken(storedRefreshToken);

            UpdateTokensInCookies(tokens.AccessToken, tokens.RefreshToken);

            Redirect("../Index");
        }

        public async Task OnPostAsync(string email, string password)
        {
            User? currentUser = await _signInManager.UserManager.FindByEmailAsync(email);

            if (currentUser is not null)
            {
                if (await _signInManager.UserManager.CheckPasswordAsync(currentUser, password))
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
                        Redirect("../Error");

                    // save tokens in cookies
                    UpdateTokensInCookies(accessToken, refreshToken.Token!);

                    Redirect("/");
                }
                else Unauthorized();
            }
            else NotFound();
        }

        private void UpdateTokensInCookies(string accessToken, string refreshToken)
        {
            // Store jwt token inside cookies too
            HttpContext.Response.Cookies.Append(TokenConst.AccessToken, accessToken, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax, Secure = true });
            HttpContext.Response.Cookies.Append(TokenConst.RefreshToken, refreshToken, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax, Secure = true });
        }
    }
}
