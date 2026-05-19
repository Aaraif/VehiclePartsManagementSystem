using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Models;

namespace VehiclePartsManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Part> Parts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Part>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            /* RELATIONSHIP */

            modelBuilder.Entity<Part>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Parts)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
