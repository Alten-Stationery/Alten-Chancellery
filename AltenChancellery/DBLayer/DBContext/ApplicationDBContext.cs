using DBLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemOffice> ItemOffice { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           

            //Configurazione della chiave composta per Enrollment
            builder.Entity<ItemOffice>()
                .HasKey(e => new { e.OfficeId, e.ItemId });

            // Configurazione delle relazioni
            builder.Entity<ItemOffice>()
                .HasOne(e => e.Office)
                .WithMany(s => s.ItemOffices)
                .HasForeignKey(e => e.OfficeId);

            builder.Entity<ItemOffice>()
                .HasOne(e => e.Item)
                .WithMany(c => c.ItemOffices)
                .HasForeignKey(e => e.ItemId);
        }

    }
}
