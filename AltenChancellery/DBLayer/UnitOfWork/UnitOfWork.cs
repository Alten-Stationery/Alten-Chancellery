using DBLayer.DBContext;
using DBLayer.Repositories.Implementations;
using DBLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;



        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            OfficeRepository = new OfficeRepository(_context);

        }

        public IOfficeRepository OfficeRepository { get; private set; }

        public async Task<int> Save()
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
