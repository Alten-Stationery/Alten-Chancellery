using DBLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Constants.Auth;
using ServiceLayer.Services.Interfaces;

namespace AltenChancellery.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITokenService _tokenService;

        public IndexModel(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<IActionResult> OnGet()
        {
            // get jwt access token
            HttpContext.Request.Cookies.TryGetValue(TokenConst.AccessToken, out string? accessToken);

            if (accessToken is null || string.IsNullOrEmpty(accessToken))
                return Redirect("/SignIn/Login");

            // check if access token is not valid, then check for refresh token
            if (_tokenService.IsAccessTokenValid(accessToken))
                return Page();

            // Check for refresh token
            HttpContext.Request.Cookies.TryGetValue(TokenConst.RefreshToken, out string? refreshToken);

            RefreshToken? storedRefreshToken = _tokenService.GetTokenByStringValue(refreshToken!);

            if (storedRefreshToken == null || storedRefreshToken.Token != refreshToken)
                return Redirect("/Error");

            bool isRefreshTokenValid = _tokenService.IsRefreshTokenValid(refreshToken!);

            if (!isRefreshTokenValid)
                return Redirect("/SignIn/Login");

            await _tokenService.RefreshToken(storedRefreshToken);

            return Page();
        }
    }
}
