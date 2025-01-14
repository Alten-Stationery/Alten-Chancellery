using DBLayer.DBContext;
using DBLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public T Create(T entity)
        {
            try
            {
                _context.Attach(entity).State = EntityState.Added;

                return entity;
            }
            catch
            {
                throw;
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                EntityEntry result = _context.Remove(entity);

                if (result.State == EntityState.Deleted)
                    return true;
                else return false;
            }
            catch
            {
                throw;
            }
        }

        public T? Find<K>(K id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<T?> FindAsync<K>(K id)
        {
            return await _dbSet.FindAsync(id);
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

        public bool Update(T entity)
        {
            try
            {
                var result = _dbSet.Update(entity);

                return result.State == EntityState.Modified || result.State == EntityState.Added;
            }
            catch
            {
                throw;
            }
        }
    }
}
