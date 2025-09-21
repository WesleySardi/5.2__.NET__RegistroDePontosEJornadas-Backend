using ProjetoBMA.DTOs;
using ProjetoBMA.Utils;

namespace ProjetoBMA.Services
{
    public interface ITimeEntryService
    {
        Task<PagedResult<TimeEntryDto>> GetAllAsync(TimeEntryQueryParameters qp, CancellationToken ct = default);
        Task<TimeEntryDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<TimeEntryDto> CreateAsync(CreateTimeEntryDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateTimeEntryDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
