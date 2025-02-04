using DBLayer.DBContext;
using DBLayer.Repositories.Implementations;
using DBLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace DBLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public INotificationRepository NotificationRepository { get; set; }
        public IUserRepository UserRepository { get; private set; }
        public IRefreshTokenRepository RefreshTokenRepository { get; private set; }
        public IOfficeRepository OfficeRepository { get; private set; }
        public IItemOfficeRepository ItemOfficeRepository { get; private set; }
        public IItemRepository ItemRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }

        private readonly ApplicationDBContext _context;

        public UnitOfWork(ApplicationDBContext context, IUserRepository userRepo)
        {
            _context = context;

            UserRepository = userRepo;
            RefreshTokenRepository = new RefreshTokenRepository(_context);
            OfficeRepository = new OfficeRepository(_context);
            ItemOfficeRepository = new ItemOfficeRepository(_context);
            ItemRepository = new ItemRepository(_context);
            NotificationRepository = new NotificationRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
