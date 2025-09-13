using Microsoft.AspNetCore.Mvc;
using MotorDiniz.API.Utilities;
using MotorDiniz.Application.DTOs.DeliveryRider;
using MotorDiniz.Application.Interfaces;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.API.Controllers
{
    [ApiController]
    public sealed class DeliveryRidersController : ControllerBase
    {
        private readonly IDeliveryRiderService _service;

        public DeliveryRidersController(IDeliveryRiderService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cadastra um novo entregador
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("entregadores")]
        public async Task<IActionResult> Create([FromBody] DeliveryRiderCreateDto req, CancellationToken ct)
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
        /// Enviar foto da CNH 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("entregadores/{id}/cnh")]
        public async Task<IActionResult> UploadCnh([FromRoute] string id, [FromBody] DeliveryRiderCnhUploadDto req, CancellationToken ct)
        {
            try
            {
                await _service.UploadCnhAsync(id, req, ct);
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
    }
}
