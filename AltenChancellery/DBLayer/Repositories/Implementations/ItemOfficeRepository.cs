using DBLayer.DBContext;
using DBLayer.Models;
using DBLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Repositories.Implementations
{
    public class ItemOfficeRepository : GenericRepository<ItemOffice, int>, IItemOfficeRepository
    {
        public ItemOfficeRepository(ApplicationDBContext context) : base(context) 
        {
        
        }

        public async Task<ItemOffice> GetItemOfficeById(int officeId, int itemId)
        {
            try 
            {
                var itemOffice = await _dbSet
                    .Where(x => x.OfficeId == officeId && x.ItemId == itemId)
                    .FirstOrDefaultAsync();
                return itemOffice;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<List<Item>> GetItemFromOffice(int officeId)
        {
            try 
            {
                var itemList = await _dbSet
                    .Where(x => x.OfficeId == officeId)
                    .Select(x => x.Item)
                    .ToListAsync();
                return itemList;
            }
            catch 
            {
                throw;
            }
        }

        
    }
}
