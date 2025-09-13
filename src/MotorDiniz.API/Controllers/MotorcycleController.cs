using Microsoft.AspNetCore.Mvc;
using MotorDiniz.Application.DTOs.Motorcycle;
using MotorDiniz.Application.Interfaces;
using MotorDiniz.Domain.Validation;
using MotorDiniz.API.Utilities;

namespace MotorDiniz.Api.Controllers
{
    [ApiController]
    public sealed class MotorcyclesController : ControllerBase
    {
        private readonly IMotorcycleService _service;

        public MotorcyclesController(IMotorcycleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cadastra uma nova moto
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("motos")]
        public async Task<IActionResult> Create([FromBody] MotorcycleCreateDto req, CancellationToken ct)
        {
            try
            {
                await _service.CreateAsync(req, ct);
                return StatusCode(201);
            }
            catch (DomainExceptionValidation)
            {
                return BadRequest(ApiResponse.InvalidData());
            }
        }

        /// <summary>
        /// Consulta motos existentes, filtrando pela placa (opcional)
        /// </summary>
        /// <param name="plate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("motos")]
        public async Task<ActionResult<IReadOnlyList<MotorcycleViewDto>>> Search([FromQuery(Name = "placa")] string? plate, CancellationToken ct)
        {
            var result = await _service.SearchAsync(plate, ct);
            return Ok(result);
        }
        /// <summary>
        /// Modificar a placa de uma moto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("motos/{id}/placa")]
        public async Task<IActionResult> UpdatePlate([FromRoute] string id, [FromBody] UpdatePlateDto req, CancellationToken ct)
        {
            try
            {
                await _service.UpdatePlateAsync(id, req, ct);
                return Ok(ApiResponse.PlateUpdated());
            }
            catch (DomainExceptionValidation)
            {
                return BadRequest(ApiResponse.InvalidData());
            }
        }
        /// <summary>
        /// Consultar moto existente por identificador
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("motos/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(ApiResponse.MalformedRequest());
            }

            var moto = await _service.GetByIdAsync(id, ct);
            if (moto is null)
            {
                return NotFound(ApiResponse.MotorcycleNotFound());
            }

            return Ok(moto);
        }
        /// <summary>
        /// Deletar uma moto existente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete("motos/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken ct)
        {
            try
            {
                await _service.DeleteAsync(id, ct);
                return NoContent();
            }
            catch (DomainExceptionValidation)
            {
                return BadRequest(ApiResponse.InvalidData());
            }
        }
    }
}
