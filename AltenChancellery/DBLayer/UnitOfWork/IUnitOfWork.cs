using DBLayer.Repositories.Interfaces;

namespace DBLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRefreshTokenRepository RefreshTokenRepo { get; }
        IUserRepository UserRepo { get; }

        int Save();
        Task<int> SaveAsync();
    }
}
