using AutoMapper;
using ProjetoBMA.Domain.Entities;
using ProjetoBMA.DTOs.Commands;
using ProjetoBMA.DTOs.Queries;
using ProjetoBMA.DTOs.Results;
using ProjetoBMA.DTOs.ViewModels;
using ProjetoBMA.Repositories.Interfaces;
using ProjetoBMA.Services.Interfaces;
using ProjetoBMA.Utils;

namespace ProjetoBMA.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IMapper _mapper;

        public TimeEntryService(ITimeEntryRepository repo, IMapper mapper)
        {
            _timeEntryRepository = repo;
            _mapper = mapper;
        }

        public async Task<TimeEntryResult> CreateAsync(CreateTimeEntryCommand command, CancellationToken ct = default)
        {
            TimeEntryHelper.EnsureValidType(command.Type);

            var entity = _mapper.Map<TimeEntry>(command);
            entity.Id = Guid.NewGuid();

            await _timeEntryRepository.AddAsync(entity, ct);
            await _timeEntryRepository.SaveChangesAsync(ct);

            return _mapper.Map<TimeEntryResult>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _timeEntryRepository.GetByIdAsync(id, ct);
            if (existing == null) return false;

            await _timeEntryRepository.DeleteAsync(existing, ct);
            await _timeEntryRepository.SaveChangesAsync(ct);

            return true;
        }

        public async Task<TimeEntryViewModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var e = await _timeEntryRepository.GetByIdAsync(id, ct);
            return e == null ? null : _mapper.Map<TimeEntryViewModel>(e);
        }

        public async Task<PagedResult<TimeEntryViewModel>> GetAllAsync(
            TimeEntryQueryParametersQuery query,
            CancellationToken ct = default)
        {
            var pagedResult = await _timeEntryRepository.GetAllAsync(query, ct).ToPagedResultAsync(query.Page, query.PageSize, x => x, ct);

            var dtoItems = pagedResult.Items.Select(x => _mapper.Map<TimeEntryViewModel>(x)).ToList();

            return new PagedResult<TimeEntryViewModel>
            {
                Items = dtoItems,
                TotalCount = pagedResult.TotalCount,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize
            };
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateTimeEntryCommand command, CancellationToken ct = default)
        {
            TimeEntryHelper.EnsureValidType(command.Type);

            var existing = await _timeEntryRepository.GetByIdAsync(id, ct);
            if (existing == null) return false;

            _mapper.Map(command, existing);
            await _timeEntryRepository.UpdateAsync(existing, ct);
            await _timeEntryRepository.SaveChangesAsync(ct);

            return true;
        }
    }
}
