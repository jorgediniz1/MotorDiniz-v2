// src/MotorDiniz.API/Controllers/RentalsController.cs
using Microsoft.AspNetCore.Mvc;
using MotorDiniz.API.Utilities; // ApiResponse
using MotorDiniz.Application.DTOs.Rental;
using MotorDiniz.Application.Interfaces;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.API.Controllers
{
    [ApiController]
    public sealed class RentalsController : ControllerBase
    {
        private readonly IRentalService _service;
        public RentalsController(IRentalService service) => _service = service;

        /// <summary>
        /// Aluga uma moto
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("locacao")]
        public async Task<IActionResult> Create([FromBody] RentalCreateDto req, CancellationToken ct)
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
            catch (ArgumentException)
            {
                return BadRequest(ApiResponse.InvalidData());
            }
        }

        /// <summary>
        /// Consulta uma locação pelo Identificador
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("locacao/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(ApiResponse.InvalidData());

            var res = await _service.GetByIdAsync(id, ct);
            if (res is null)
                return NotFound(ApiResponse.RentalNotFound());

            return Ok(res);
        }

        /// <summary>
        /// Informa a data de devolução da moto alugada
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("locacao/{id}/devolucao")]
        public async Task<IActionResult> InformReturn([FromRoute] string id, [FromBody] ReturnDateDto req, CancellationToken ct)
        {
            try
            {
                await _service.InformReturnAsync(id, req, ct);
                return Ok(ApiResponse.ReturnDateInformed());
            }
            catch (DomainExceptionValidation)
            {
                return BadRequest(ApiResponse.InvalidData());
            }
        }
    }
}
