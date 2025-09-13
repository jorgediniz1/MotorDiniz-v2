using MediatR;
using MotorDiniz.Application.CQRS.Motorcycles.Queries;
using MotorDiniz.Application.DTOs.Motorcycle;
using MotorDiniz.Application.Extensions;
using MotorDiniz.Application.Interfaces;


namespace MotorDiniz.Application.Services
{
    public sealed class MotorcycleService : IMotorcycleService
    {
        private readonly IMediator _mediator;
        public MotorcycleService(IMediator mediator) => _mediator = mediator;

        public Task CreateAsync(MotorcycleCreateDto dto, CancellationToken cancellationToken)
            => _mediator.Send(dto.ToCreateCommand()!, cancellationToken);

        public Task<IReadOnlyList<MotorcycleViewDto>> SearchAsync(string? plate, CancellationToken cancellationToken)
            => _mediator.Send(new SearchMotorcyclesQuery { Plate = plate }, cancellationToken);

        public Task<MotorcycleViewDto?> GetByIdAsync(string identifier, CancellationToken cancellationToken)
            => _mediator.Send(new GetMotorcycleByIdQuery { Identifier = identifier }, cancellationToken);

        public Task UpdatePlateAsync(string identifier, UpdatePlateDto dto, CancellationToken cancellationToken)
            => _mediator.Send(dto.ToUpdatePlateCommand(identifier), cancellationToken);

        public Task DeleteAsync(string identifier, CancellationToken cancellationToken)
            => _mediator.Send(identifier.ToDeleteCommand(), cancellationToken);
      
    }
}
