using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjetoBMA.DTOs;
using ProjetoBMA.Services;
using ProjetoBMA.Utils;

namespace ProjetoBMA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ITimeEntryService _service;
        private readonly ILogger<TimeEntriesController> _logger;
        private readonly IMapper _mapper;

        public TimeEntriesController(ITimeEntryService service, ILogger<TimeEntriesController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TimeEntryDto>>> Get([FromQuery] TimeEntryQueryParameters qp, CancellationToken ct)
        {
            var result = await _service.GetAllAsync(qp, ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TimeEntryDto>> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _service.GetByIdAsync(id, ct);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<TimeEntryDto>> Create([FromBody] CreateTimeEntryDto dto, CancellationToken ct)
        {
            var created = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTimeEntryDto dto, CancellationToken ct)
        {
            var updated = await _service.UpdateAsync(id, dto, ct);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
