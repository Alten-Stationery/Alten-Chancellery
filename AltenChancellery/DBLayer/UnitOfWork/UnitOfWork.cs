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
        public IOfficeRepository OfficeRepository { get; private set; }

        private readonly ApplicationDBContext _context;

        public UnitOfWork(ApplicationDBContext context, IUserRepository userRepo)
        {
            _context = context;

            UserRepo = userRepo;
            RefreshTokenRepo = new RefreshTokenRepository(_context);
            OfficeRepository = new OfficeRepository(_context);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
