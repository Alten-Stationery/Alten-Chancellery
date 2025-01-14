using DBLayer.Models;

namespace DBLayer.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, int>
    {
        bool CreateOrUpdate(RefreshToken refreshToken);
        RefreshToken? GetByTokenString(string refreshToken);
        RefreshToken? GetByUserId(string userId);
        string GetUserIdByTokenString(string refreshToken);
    }
}
