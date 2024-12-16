using DBLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.DBContext
{
    public class ApplicationDBContext: IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Office> Office { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .Property(e => e.Id)
                .HasDefaultValue("NEWID()");
        }

    }
}
