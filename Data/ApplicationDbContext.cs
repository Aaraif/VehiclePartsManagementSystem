using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Models;

namespace VehiclePartsManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /* TABLES */

        public DbSet<Part> Parts { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* PRICE PRECISION */

            modelBuilder.Entity<Part>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            /* PART ↔ SUPPLIER RELATIONSHIP */

            modelBuilder.Entity<Part>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Parts)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);

            /* SALE ↔ CUSTOMER RELATIONSHIP */

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            /* SALE ↔ PART RELATIONSHIP */

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Part)
                .WithMany()
                .HasForeignKey(s => s.PartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}