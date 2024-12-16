using DBLayer.DBContext;
using DBLayer.Models;
using DBLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Repositories.Implementations
{
    public class ItemRepository: GenericRepository<Item, int> , IItemRepository
    {
        public ItemRepository(ApplicationDBContext context) : base(context) 
        { }
    }
}
