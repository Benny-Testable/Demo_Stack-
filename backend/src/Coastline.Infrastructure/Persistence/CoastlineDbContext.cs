using Coastline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coastline.Infrastructure.Persistence;

public class CoastlineDbContext : DbContext
{
    public CoastlineDbContext(DbContextOptions<CoastlineDbContext> options) : base(options) { }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<ClassSession> ClassSessions => Set<ClassSession>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Site> Sites => Set<Site>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("members");
            entity.HasKey(m => m.Id);
            entity.HasIndex(m => m.Email).IsUnique();
            entity.Property(m => m.FullName).HasMaxLength(200).IsRequired();
            entity.Property(m => m.Email).HasMaxLength(200).IsRequired();
            entity.Metadata.FindNavigation(nameof(Member.Bookings))!
                  .SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<ClassSession>(entity =>
        {
            entity.ToTable("class_sessions");
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => s.StartsAt);
            entity.Property(s => s.Title).HasMaxLength(160).IsRequired();
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("bookings");
            entity.HasKey(b => b.Id);
            entity.HasIndex(b => new { b.MemberId, b.ClassSessionId }).IsUnique();
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.ToTable("sites");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).HasMaxLength(160).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
