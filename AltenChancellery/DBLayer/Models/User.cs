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
        public String Name { get; set; }
        public String Surname { get; set; }
      
       
    }
}
