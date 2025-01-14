using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Models
{
    public  class ItemOffice
    {
        public int ItemId { get; set; }
        public int OfficeId { get; set; }
        public virtual Office Office { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
    }
}
