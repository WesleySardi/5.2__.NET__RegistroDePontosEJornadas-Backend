using ProjetoBMA.DTOs.Commands;
using ProjetoBMA.DTOs.Queries;
using ProjetoBMA.DTOs.Results;
using ProjetoBMA.DTOs.ViewModels;
using ProjetoBMA.Utils;

namespace ProjetoBMA.Services.Interfaces
{
    public interface ITimeEntryService
    {
        Task<PagedResult<TimeEntryViewModel>> GetAllAsync(TimeEntryQueryParametersQuery query, CancellationToken ct = default);
        Task<TimeEntryViewModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<TimeEntryResult> CreateAsync(CreateTimeEntryCommand command, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateTimeEntryCommand command, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
