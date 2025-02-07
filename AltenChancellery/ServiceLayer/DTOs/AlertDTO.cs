using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTOs
{
    public class AlertDTO
    {
        public int AlertId { get; set; }
        public int ItemId { get; set; }
        public int OfficeId { get; set; }
        public DateTime Date { get; set; }
    }
}
