using ProjetoBMA.Domain.Entities;
using ProjetoBMA.DTOs.Queries;

namespace ProjetoBMA.Repositories.Interfaces
{
    public interface ITimeEntryRepository
    {
        IQueryable<TimeEntry> Query();
        Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(TimeEntry entry, CancellationToken ct = default);
        Task UpdateAsync(TimeEntry entry, CancellationToken ct = default);
        Task DeleteAsync(TimeEntry entry, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        IQueryable<TimeEntry> GetAllAsync(TimeEntryQueryParametersQuery query, CancellationToken ct = default);
    }
}
