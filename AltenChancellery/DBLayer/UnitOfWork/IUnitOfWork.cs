using DBLayer.Repositories.Interfaces;

namespace DBLayer.UnitOfWork
{
    public interface IUnitOfWork :IDisposable
    {

        IRefreshTokenRepository RefreshTokenRepo { get; }
        IOfficeRepository OfficeRepository { get; }
        IUserRepository UserRepo { get; }

        int Save();
        Task<int> SaveAsync();

    }
}
