using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Models
{
    public class Alert
    {
        public int AlertId { get; set; }
        public int ItemId { get; set; }
        public int OfficeId { get; set; }
        public DateTime Date { get; set; }

        public virtual ItemOffice ItemOffice { get; set; } = null!;
    }

}
