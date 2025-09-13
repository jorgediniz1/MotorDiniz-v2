using MediatR;
using MotorDiniz.Application.CQRS.Rentals.Commands;
using MotorDiniz.Application.CQRS.Rentals.Queries;
using MotorDiniz.Application.DTOs.Rental;
using MotorDiniz.Application.Extensions;
using MotorDiniz.Application.Interfaces;



namespace MotorDiniz.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IMediator _mediator;
        public RentalService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task CreateAsync(RentalCreateDto dto, CancellationToken cancellationToken)
            => _mediator.Send(dto.ToCreateCommand(), cancellationToken);

        public Task<RentalViewDto?> GetByIdAsync(string identifier, CancellationToken cancellationToken)
            => _mediator.Send(new GetRentalByIdQuery { Identifier = identifier }, cancellationToken);

        public Task InformReturnAsync(string identifier, ReturnDateDto dto, CancellationToken cancellationToken)
            => _mediator.Send(new InformReturnDateCommand { Identifier = identifier, ReturnDate = dto.ReturnDate }, cancellationToken);
    }
}
