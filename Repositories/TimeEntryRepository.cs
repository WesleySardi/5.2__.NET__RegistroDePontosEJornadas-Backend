using Microsoft.EntityFrameworkCore;
using ProjetoBMA.Data;
using ProjetoBMA.Entities;
using ProjetoBMA.Utils;

namespace ProjetoBMA.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly AppDbContext _ctx;

        public TimeEntryRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task AddAsync(TimeEntry entry, CancellationToken ct = default)
        {
            await _ctx.TimeEntries.AddAsync(entry, ct);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(TimeEntry entry, CancellationToken ct = default)
        {
            _ctx.TimeEntries.Remove(entry);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _ctx.TimeEntries.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<PagedResult<TimeEntry>> GetPagedAsync(string? employeeId, string? type, DateTime? from, DateTime? to, int page, int pageSize, string? sort, CancellationToken ct = default)
        {
            var query = _ctx.TimeEntries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(employeeId))
                query = query.Where(x => x.EmployeeId == employeeId);

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(x => x.Type == type);

            if (from.HasValue)
                query = query.Where(x => x.Timestamp >= from.Value);

            if (to.HasValue)
                query = query.Where(x => x.Timestamp <= to.Value);

            // Sorting
            query = sort switch
            {
                "timestamp_desc" => query.OrderByDescending(x => x.Timestamp),
                "timestamp_asc" => query.OrderBy(x => x.Timestamp),
                "employeeName_asc" => query.OrderBy(x => x.EmployeeName),
                "employeeName_desc" => query.OrderByDescending(x => x.EmployeeName),
                _ => query.OrderByDescending(x => x.Timestamp)
            };

            var total = await query.CountAsync(ct);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

            return new PagedResult<TimeEntry>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task UpdateAsync(TimeEntry entry, CancellationToken ct = default)
        {
            _ctx.TimeEntries.Update(entry);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            return await _ctx.TimeEntries.AnyAsync(x => x.Id == id, ct);
        }
    }
}
