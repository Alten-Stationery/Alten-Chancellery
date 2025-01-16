using DBLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Constants.Auth;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Interfaces;
using System.Collections;

namespace AltenChancellery.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITokenService _tokenService;

        public IDictionary<string, int> Items { get; set; } // TODO: change type to items

        public IndexModel(ITokenService tokenService)
        {
            _tokenService = tokenService;

            Items = new Dictionary<string, int>();
        }

        public async Task<IActionResult> OnGet()
        {
            Items = new Dictionary<string, int>()
            {
                { "Acqua", -6 },
                { "Medicine", 1 },
                { "Cacciavite", 2 },
                { "Quaderni", 7 },
                { "Penne", 10 },
                { "Subwoofer", 2 },
                { "Rum", 3 },
            };

            return await TokenCheckProceedings();
        }

        private async Task<IActionResult> TokenCheckProceedings()
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

            RefreshTokenDTO? storedRefreshToken = _tokenService.GetTokenByStringValue(refreshToken!);

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
