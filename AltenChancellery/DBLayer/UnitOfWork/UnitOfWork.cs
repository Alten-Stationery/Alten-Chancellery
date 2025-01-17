using DBLayer.DBContext;
using DBLayer.Repositories.Implementations;
using DBLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace DBLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepo { get; private set; }
        public IRefreshTokenRepository RefreshTokenRepo { get; private set; }
        

        private readonly ApplicationDBContext _context;

        public UnitOfWork(ApplicationDBContext context, IUserRepository userRepo)
        {
            _context = context;

            UserRepo = userRepo;
            RefreshTokenRepo = new RefreshTokenRepository(_context);
            OfficeRepository = new OfficeRepository(_context);
            itemOfficeRepository = new ItemOfficeRepository(_context);
            itemRepository = new ItemRepository(_context);
            alertRepository = new AlertRepository(_context);

        }

        public IOfficeRepository OfficeRepository { get; private set; }
        public IItemOfficeRepository itemOfficeRepository { get; private set; }
        public IItemRepository itemRepository { get; private set; }
        public IAlertRepository alertRepository { get; private set; }

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
