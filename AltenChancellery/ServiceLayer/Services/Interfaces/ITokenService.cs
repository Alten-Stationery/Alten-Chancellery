using DBLayer.Models;
using ServiceLayer.DTOs;
using ServiceLayer.DTOs.Common;

namespace ServiceLayer.Services.Interfaces
{
    public interface ITokenService
    {
        bool GenerateAndSaveTokens(User currentUser, out string accessToken, out string refreshToken);
        Task<Tokens> RefreshToken(RefreshTokenDTO currentRefreshToken);
        RefreshTokenDTO? GetTokenByStringValue(string refreshToken);
        bool DeleteToken(string refreshToken);
        bool IsRefreshTokenValid(string refreshToken);
        bool IsAccessTokenValid(string accessToken);
        Task<UserDTO> GetUserByRefreshToken(string refreshToken);
    }
}
