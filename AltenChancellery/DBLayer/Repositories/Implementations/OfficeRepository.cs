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
    public class OfficeRepository : GenericRepository<Office>,  IOfficeRepository
    {
        public OfficeRepository(ApplicationDBContext context) : base(context) 
        {
        }    

    }
}
