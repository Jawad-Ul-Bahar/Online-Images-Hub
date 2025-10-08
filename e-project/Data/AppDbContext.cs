using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using e_project.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<e_project.Models.User> User { get; set; } = default!;
        public DbSet<e_project.Models.PhotoOrderItem> PhotoOrderItem { get; set; }
    public DbSet<e_project.Models.Order> Order { get; set; }
    public DbSet<PrintSizePrice> PrintSizePrice { get; set; }

    public DbSet<Admin> Admins { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set precision: up to 10 digits, 2 of them after decimal
        modelBuilder.Entity<PrintSizePrice>()
            .Property(p => p.Price)
            .HasPrecision(10, 2); // or use .HasColumnType("decimal(10,2)");

        // Seed data
        modelBuilder.Entity<PrintSizePrice>().HasData(
            new PrintSizePrice { Id = 1, Size = "4x6", Price = 10.00m },
            new PrintSizePrice { Id = 2, Size = "5x7", Price = 15.00m },
            new PrintSizePrice { Id = 3, Size = "8x10", Price = 25.00m }
        );
    }


}
