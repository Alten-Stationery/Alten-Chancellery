using DBLayer.DBContext;
using DBLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Repositories.Implementations
{
    public class GenericRepository<T, TID> : IGenericRepository<T, TID> where T : class
    {
        private readonly ApplicationDBContext _context;
        private protected DbSet<T> _dbSet;
        public GenericRepository(ApplicationDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                _context.Attach(entity).State = EntityState.Added;

                if (_context.Entry(entity).State == EntityState.Added)
                {
                    await _context.SaveChangesAsync();
                    return entity;
                }
                else
                {
                    throw new InvalidOperationException("Impossible to retrive the ID from th entity");
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Deleted;
                var result = await _context.SaveChangesAsync();
                return result > 0;

            }
            catch
            {
                throw;
            }
        }

        public async Task<T?> FindAsync(TID id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                var result = _dbSet.Update(entity);

                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch
            {
                throw;
            }
        }
    }
}
