using DBLayer.Repositories.Interfaces;

namespace DBLayer.UnitOfWork
{
    public interface IUnitOfWork :IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IOfficeRepository OfficeRepository { get; }
        IItemOfficeRepository ItemOfficeRepository { get; }
        IItemRepository ItemRepository { get; }
        IUserRepository UserRepository { get; }

        Task<int> SaveAsync();

    }
}
