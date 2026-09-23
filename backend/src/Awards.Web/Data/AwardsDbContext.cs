using Awards.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Awards.Web.Data;

public class AwardsDbContext : DbContext
{
    public AwardsDbContext(DbContextOptions<AwardsDbContext> options) : base(options) { }

    public DbSet<Applicant> Applicants => Set<Applicant>();
    public DbSet<Programme> Programmes => Set<Programme>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<Disbursement> Disbursements => Set<Disbursement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.HasIndex(a => a.Email).IsUnique();
            entity.Property(a => a.GradePointAverage).HasColumnType("decimal(4,2)");
            entity.Property(a => a.HouseholdIncome).HasColumnType("decimal(14,2)");
        });

        modelBuilder.Entity<Programme>(entity =>
        {
            entity.HasIndex(p => p.Code).IsUnique();
            entity.Property(p => p.AwardAmount).HasColumnType("decimal(14,2)");
            entity.Property(p => p.MinimumGpa).HasColumnType("decimal(4,2)");
            entity.Property(p => p.IncomeCeiling).HasColumnType("decimal(14,2)");
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasIndex(a => a.Reference).IsUnique();
            entity.Property(a => a.EligibilityScore).HasColumnType("decimal(6,2)");

            entity.HasOne(a => a.Applicant)
                  .WithMany(p => p.Applications)
                  .HasForeignKey(a => a.ApplicantId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Programme)
                  .WithMany(p => p.Applications)
                  .HasForeignKey(a => a.ProgrammeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Disbursement>(entity =>
        {
            entity.Property(d => d.Amount).HasColumnType("decimal(14,2)");
            entity.Property(d => d.AmountPaid).HasColumnType("decimal(14,2)");

            entity.HasOne(d => d.Application)
                  .WithMany(a => a.Disbursements)
                  .HasForeignKey(d => d.ApplicationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
