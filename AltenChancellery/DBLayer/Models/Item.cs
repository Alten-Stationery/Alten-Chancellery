using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Availability { get; set; }
        public int MinimumAvailability { get; set; }
        public virtual List<ItemOffice> ItemOffices { get; set; }
    }
}
