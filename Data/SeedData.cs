using Microsoft.EntityFrameworkCore;
using ProjetoBMA.Entities;

namespace ProjetoBMA.Data
{
    public static class SeedData
    {
        public static async Task EnsureSeedDataAsync(AppDbContext context)
        {
            if (await context.TimeEntries.AnyAsync()) return;

            var list = new List<TimeEntry>
            {
                new TimeEntry
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = "EMP001",
                    EmployeeName = "João Silva",
                    Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(8),
                    Type = "Entrada",
                    Location = "Portaria A",
                    Notes = "Entrada padrão",
                    CreatedAt = DateTime.UtcNow
                },
                new TimeEntry
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = "EMP001",
                    EmployeeName = "João Silva",
                    Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(12),
                    Type = "Saída",
                    CreatedAt = DateTime.UtcNow
                },
                new TimeEntry
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = "EMP002",
                    EmployeeName = "Maria Oliveira",
                    Timestamp = DateTime.UtcNow.AddHours(-2),
                    Type = "Entrada",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.TimeEntries.AddRange(list);
            await context.SaveChangesAsync();
        }
    }
}
