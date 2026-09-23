using Estates.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Estates.Web.Data;

public class EstatesDbContext : DbContext
{
    public EstatesDbContext(DbContextOptions<EstatesDbContext> options) : base(options) { }

    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Lease> Leases => Set<Lease>();
    public DbSet<PaymentSchedule> PaymentSchedules => Set<PaymentSchedule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasIndex(p => p.BlockNumber);
            entity.Property(p => p.FloorAreaSqFt).HasColumnType("decimal(12,2)");
        });

        modelBuilder.Entity<Lease>(entity =>
        {
            entity.HasIndex(l => l.Reference).IsUnique();
            entity.Property(l => l.BaseAnnualRent).HasColumnType("decimal(14,2)");
            entity.Property(l => l.ServiceCharge).HasColumnType("decimal(14,2)");
            entity.Property(l => l.DepositHeld).HasColumnType("decimal(14,2)");

            entity.HasOne(l => l.Property)
                  .WithMany(p => p.Leases)
                  .HasForeignKey(l => l.PropertyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Tenant)
                  .WithMany(t => t.Leases)
                  .HasForeignKey(l => l.TenantId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PaymentSchedule>(entity =>
        {
            entity.Property(p => p.AmountDue).HasColumnType("decimal(14,2)");
            entity.Property(p => p.AmountPaid).HasColumnType("decimal(14,2)");

            entity.HasOne(p => p.Lease)
                  .WithMany(l => l.Payments)
                  .HasForeignKey(p => p.LeaseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
