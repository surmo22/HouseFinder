using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HouseFinderBackEnd.Data.Buildings;
using System.Reflection.Emit;

namespace HouseFinderBackEnd.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Property>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
            builder.Entity<Property>()
                .HasOne(p => p.User)
                .WithMany(u => u.Properties)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Property>()
                .HasMany(p => p.Watchers)
                .WithMany(u => u.WatchList)
                .UsingEntity<Dictionary<string, object>>(
            "PropertyUser",
            j => j
                .HasOne<User>()
                .WithMany()
                .HasForeignKey("WatchersId")
                .OnDelete(DeleteBehavior.Restrict),
            j => j
                .HasOne<Property>()
                .WithMany()
                .HasForeignKey("WatchListId")
                .OnDelete(DeleteBehavior.Restrict));;
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
