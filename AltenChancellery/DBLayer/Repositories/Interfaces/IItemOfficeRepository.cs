using DBLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Repositories.Interfaces
{
    public interface IItemOfficeRepository : IGenericRepository<ItemOffice, int>
    {
        Task<List<Item>> GetItemFromOffice(int officeId);
        Task<ItemOffice> GetItemOfficeById(int officeId, int itemId);
        
    }
}
