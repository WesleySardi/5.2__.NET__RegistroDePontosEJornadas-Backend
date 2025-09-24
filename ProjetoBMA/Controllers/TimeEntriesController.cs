using Microsoft.AspNetCore.Mvc;
using ProjetoBMA.DTOs.Commands;
using ProjetoBMA.DTOs.Queries;
using ProjetoBMA.DTOs.Results;
using ProjetoBMA.DTOs.ViewModels;
using ProjetoBMA.Services.Interfaces;
using ProjetoBMA.Utils;

namespace ProjetoBMA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ITimeEntryService _service;
        private readonly ILogger<TimeEntriesController> _logger;

        public TimeEntriesController(ITimeEntryService service, ILogger<TimeEntriesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obt�m uma lista paginada de registros de ponto.
        /// </summary>
        /// <param name="qp">Par�metros de consulta para filtro, ordena��o e pagina��o.</param>
        /// <param name="ct">Token de cancelamento da requisi��o.</param>
        /// <returns>Retorna uma p�gina de <see cref="TimeEntryViewModel"/> com total de registros e informa��es de pagina��o.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TimeEntryViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<TimeEntryViewModel>>> Get([FromQuery] TimeEntryQueryParametersQuery query, CancellationToken ct)
        {
            var result = await _service.GetAllAsync(query, ct);
            return Ok(result);
        }

        /// <summary>
        /// Obt�m um registro de ponto pelo ID.
        /// </summary>
        /// <param name="id">ID do registro de ponto.</param>
        /// <param name="ct">Token de cancelamento da requisi��o.</param>
        /// <returns>Retorna o registro correspondente ou 404 se n�o encontrado.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TimeEntryViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TimeEntryViewModel>> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _service.GetByIdAsync(id, ct);
            if (dto == null)
            {
                _logger.LogWarning("TimeEntry n�o encontrado com ID {Id}", id);
                return NotFound();
            }

            return Ok(dto);
        }

        /// <summary>
        /// Cria um novo registro de ponto.
        /// </summary>
        /// <param name="dto">Dados para cria��o do registro.</param>
        /// <param name="ct">Token de cancelamento da requisi��o.</param>
        /// <returns>Retorna o registro criado com status 201 Created.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TimeEntryResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimeEntryResult>> Create([FromBody] CreateTimeEntryCommand command, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(command, ct);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza um registro de ponto existente.
        /// </summary>
        /// <param name="id">ID do registro a ser atualizado.</param>
        /// <param name="dto">Dados para atualiza��o do registro.</param>
        /// <param name="ct">Token de cancelamento da requisi��o.</param>
        /// <returns>204 NoContent se atualizado com sucesso ou 404 NotFound se o registro n�o existir.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTimeEntryCommand command, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, command, ct);
            if (!updated) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Exclui um registro de ponto pelo ID.
        /// </summary>
        /// <param name="id">ID do registro a ser exclu�do.</param>
        /// <param name="ct">Token de cancelamento da requisi��o.</param>
        /// <returns>204 NoContent se exclu�do com sucesso ou 404 NotFound se o registro n�o existir.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
