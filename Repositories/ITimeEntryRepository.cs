using ProjetoBMA.Entities;
using ProjetoBMA.Utils;

namespace ProjetoBMA.Repositories
{
    public interface ITimeEntryRepository
    {
        Task<PagedResult<TimeEntry>> GetPagedAsync(string? employeeId, string? type, DateTime? from, DateTime? to,
            int page, int pageSize, string? sort, CancellationToken ct = default);
        Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(TimeEntry entry, CancellationToken ct = default);
        Task UpdateAsync(TimeEntry entry, CancellationToken ct = default);
        Task DeleteAsync(TimeEntry entry, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    }
}
