using DBLayer.Models;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces
{
    public interface ITokenService
    {
        bool SaveRefreshToken(RefreshToken token);
        string GenerateJwtAccessToken(User user, IdentityRole role);
        RefreshToken GenerateRefreshToken(User user);
        Task<Tokens> RefreshToken(RefreshToken currentRefreshToken);
        RefreshToken? GetTokenByStringValue(string refreshToken);
        bool DeleteToken(string refreshToken);
        bool IsRefreshTokenValid(string refreshToken);
        bool IsAccessTokenValid(string accessToken);
        Task<User> GetUserByRefreshToken(string refreshToken);
    }
}
