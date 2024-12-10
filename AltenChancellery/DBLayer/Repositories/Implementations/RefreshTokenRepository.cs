using DBLayer.DBContext;
using DBLayer.Models;
using DBLayer.Repositories.Interfaces;

namespace DBLayer.Repositories.Implementations
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDBContext context) : base(context)
        {
        }

        public bool CreateOrUpdate(RefreshToken refreshToken)
        {
            RefreshToken? checkExistance = GetByUserId(refreshToken.UserId);
            bool result = false;

            if (checkExistance == null)
                result = Create(refreshToken) != null;
            else
            {
                refreshToken.UserId = checkExistance.UserId;
                _dbSet.Update(refreshToken);

                result = true;
            }

            return result;
        }

        public RefreshToken? GetByTokenString(string refreshToken) => _dbSet.SingleOrDefault(s => s.Token == refreshToken);

        public RefreshToken? GetByUserId(string userId) => _dbSet.FirstOrDefault(f => f.UserId == userId);
    }
}
