using AutoMapper;
using ProjetoBMA.DTOs;
using ProjetoBMA.Entities;
using ProjetoBMA.Repositories;
using ProjetoBMA.Utils;

namespace ProjetoBMA.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private static readonly string[] AllowedTypes = new[] { "Entrada", "Saída", "Intervalo" };

        private readonly ITimeEntryRepository _repo;
        private readonly IMapper _mapper;

        public TimeEntryService(ITimeEntryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        private void EnsureValidType(string type)
        {
            if (!AllowedTypes.Contains(type))
                throw new ArgumentException($"Tipo inválido. Valores permitidos: {string.Join(", ", AllowedTypes)}");
        }

        public async Task<TimeEntryDto> CreateAsync(CreateTimeEntryDto dto, CancellationToken ct = default)
        {
            EnsureValidType(dto.Type);

            var entity = _mapper.Map<TimeEntry>(dto);
            entity.Id = Guid.NewGuid();

            await _repo.AddAsync(entity, ct);

            return _mapper.Map<TimeEntryDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return false;

            await _repo.DeleteAsync(existing, ct);
            return true;
        }

        public async Task<TimeEntryDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var e = await _repo.GetByIdAsync(id, ct);
            return e == null ? null : _mapper.Map<TimeEntryDto>(e);
        }

        public async Task<PagedResult<TimeEntryDto>> GetAllAsync(TimeEntryQueryParameters qp, CancellationToken ct = default)
        {
            var page = Math.Max(1, qp.Page);
            var pageSize = Math.Clamp(qp.PageSize, 1, 100);

            var result = await _repo.GetPagedAsync(qp.EmployeeId, qp.Type, qp.From, qp.To, page, pageSize, qp.Sort, ct);

            var dtoItems = result.Items.Select(x => _mapper.Map<TimeEntryDto>(x)).ToList();

            return new PagedResult<TimeEntryDto>
            {
                Items = dtoItems,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateTimeEntryDto dto, CancellationToken ct = default)
        {
            EnsureValidType(dto.Type);

            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return false;

            // map fields
            existing.EmployeeId = dto.EmployeeId;
            existing.EmployeeName = dto.EmployeeName;
            existing.Timestamp = dto.Timestamp;
            existing.Type = dto.Type;
            existing.Location = dto.Location;
            existing.Notes = dto.Notes;

            await _repo.UpdateAsync(existing, ct);
            return true;
        }
    }
}
