using Microsoft.EntityFrameworkCore;
using ProjetoBMA.Domain.Entities;

namespace ProjetoBMA.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TimeEntry> TimeEntries { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TimeEntry>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.EmployeeId).IsRequired().HasMaxLength(50);
                b.Property(x => x.EmployeeName).IsRequired().HasMaxLength(200);
                b.Property(x => x.Type).IsRequired().HasMaxLength(20);
                b.Property(x => x.Timestamp).IsRequired();

                b.HasIndex(x => x.EmployeeId);
                b.HasIndex(x => x.Timestamp);
                b.HasIndex(x => x.Type);
            });
        }

        public override int SaveChanges()
        {
            ApplyAudits();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAudits();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAudits()
        {
            var entries = ChangeTracker.Entries<TimeEntry>();

            // var now = DateTime.UtcNow; --> Global
            var now = DateTime.Now;    // --> Brazil

            foreach (var e in entries)
            {
                if (e.State == EntityState.Added)
                {
                    e.Entity.CreatedAt = now;
                }
                else if (e.State == EntityState.Modified)
                {
                    e.Entity.UpdatedAt = now;
                }
            }
        }
    }
}
