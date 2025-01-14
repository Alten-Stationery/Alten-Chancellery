using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? OfficeId { get; set; }
        public virtual Office Office { get; set; }
      
       
    }
}
