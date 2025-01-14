using DBLayer.Repositories.Interfaces;

namespace DBLayer.UnitOfWork
{
    public interface IUnitOfWork :IDisposable
    {

        IRefreshTokenRepository RefreshTokenRepo { get; }
        IOfficeRepository OfficeRepository { get; }
        IItemOfficeRepository itemOfficeRepository { get; }
        IItemRepository itemRepository { get; }
        IUserRepository UserRepo { get; }

        Task<int> SaveAsync();

    }
}
