using Microsoft.EntityFrameworkCore;
using ProjetoBMA.Data;
using ProjetoBMA.Domain.Entities;
using ProjetoBMA.DTOs.Queries;
using ProjetoBMA.Repositories.Interfaces;

namespace ProjetoBMA.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly AppDbContext _context;

        public TimeEntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TimeEntry entry, CancellationToken ct = default)
        {
            await _context.TimeEntries.AddAsync(entry, ct);
        }

        public Task UpdateAsync(TimeEntry entry, CancellationToken ct = default)
        {
            _context.TimeEntries.Attach(entry);
            _context.Entry(entry).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TimeEntry entry, CancellationToken ct = default)
        {
            _context.TimeEntries.Remove(entry);
            return Task.CompletedTask;
        }

        public async Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.TimeEntries.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.TimeEntries.AnyAsync(x => x.Id == id, ct);
        }
        public IQueryable<TimeEntry> Query()
        {
            return _context.TimeEntries.AsQueryable();
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public IQueryable<TimeEntry> GetAllAsync(TimeEntryQueryParametersQuery query, CancellationToken ct = default)
        {
            var result = _context.TimeEntries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.EmployeeId))
                result = result.Where(x => x.EmployeeId == query.EmployeeId);

            if (query.Type.HasValue)
                result = result.Where(x => x.Type == query.Type);

            if (query.From.HasValue)
                result = result.Where(x => x.Timestamp >= query.From.Value);

            if (query.To.HasValue)
                result = result.Where(x => x.Timestamp <= query.To.Value);

            result = query.Sort switch
            {
                "timestamp_desc" => result.OrderByDescending(x => x.Timestamp),
                "timestamp_asc" => result.OrderBy(x => x.Timestamp),
                "employeeName_asc" => result.OrderBy(x => x.EmployeeName),
                "employeeName_desc" => result.OrderByDescending(x => x.EmployeeName),
                _ => result.OrderByDescending(x => x.Timestamp)
            };

            return result;
        }
    }
}
