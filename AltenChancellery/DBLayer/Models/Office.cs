﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Models
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public virtual List<ItemOffice> ItemOffices { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
