using DBLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Constants.Auth;
using ServiceLayer.DTOs;
using ServiceLayer.Services.Implementations;
using ServiceLayer.Services.Interfaces;
using System.Collections;

namespace AltenChancellery.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITokenService _tokenService;
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;

        public IList<ItemDTO> Items { get; set; }
        public IList<CategoryDTO> Categories { get; set; }

        public IndexModel(ITokenService tokenService, IItemService itemService, ICategoryService categoryService)
        {
            _tokenService = tokenService;
            _itemService = itemService;
            _categoryService = categoryService;

            Items = new List<ItemDTO>();
            Categories = new List<CategoryDTO>();
        }

        public async Task<IActionResult> OnGet()
        {
            var categories = await _categoryService.GetAll();
            var items = await _itemService.GetAll();

            Categories = categories.Data;
            Items = items.Data;

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
            {
                HttpContext.Response.Cookies.Delete(TokenConst.AccessToken);
                HttpContext.Response.Cookies.Delete(TokenConst.RefreshToken);
                return Redirect("/Error");
            }

            bool isRefreshTokenValid = _tokenService.IsRefreshTokenValid(refreshToken!);

            if (!isRefreshTokenValid)
                return Redirect("/SignIn/Login");

            await _tokenService.RefreshToken(storedRefreshToken);

            return Page();
        }
    }
}
