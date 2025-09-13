using MediatR;
using MotorDiniz.Application.DTOs.DeliveryRider;
using MotorDiniz.Application.Extensions;
using MotorDiniz.Application.Interfaces;

namespace MotorDiniz.Application.Services
{
    public class DeliveryRiderService : IDeliveryRiderService
    {

        private readonly IMediator _mediator;

        public DeliveryRiderService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public Task CreateAsync(DeliveryRiderCreateDto dto, CancellationToken cancellationToken)
            => _mediator.Send(dto.ToCreateCommand()!, cancellationToken);


        public Task UploadCnhAsync(string identifier, DeliveryRiderCnhUploadDto dto, CancellationToken cancellationToken)
            => _mediator.Send(dto.ToUploadCnhCommand(identifier)!, cancellationToken);


    }
}
