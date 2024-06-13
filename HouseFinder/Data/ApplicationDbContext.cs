﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HouseFinderBackEnd.Data.Buildings;

namespace HouseFinderBackEnd.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Property>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<UserPropertiesForSale> UserPropertiesForSales { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
    }
}
