using DBLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.DBContext
{
    public class ApplicationDBContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Office> Office { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemOffice> ItemOffice { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // USER
            builder.Entity<User>()
                .Property(e => e.Id)
                .HasDefaultValue("NEWID()");

            // CATEGORY
            builder.Entity<Category>(x =>
            {
                x.Property(p => p.Name).HasConversion<string>();
            });

            // ITEM OFFICE
            builder.Entity<ItemOffice>(x =>
            {
                //Configurazione della chiave composta per Enrollment
                x.HasKey(e => new { e.OfficeId, e.ItemId });

                // Configurazione delle relazioni
                x.HasOne(e => e.Office)
                .WithMany(s => s.ItemOffices)
                .HasForeignKey(e => e.OfficeId);

                x.HasOne(e => e.Item)
                .WithMany(c => c.ItemOffices)
                .HasForeignKey(e => e.ItemId);
            });

            // NOTIFICATION
            builder.Entity<Notification>(x =>
            {
                x.HasMany(n => n.Users).WithMany(u => u.Notifications);
            });
        }

    }
}
