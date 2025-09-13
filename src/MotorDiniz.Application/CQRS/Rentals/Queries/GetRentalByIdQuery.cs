using MediatR;
using MotorDiniz.Application.DTOs.Rental;

namespace MotorDiniz.Application.CQRS.Rentals.Queries
{
    public sealed class GetRentalByIdQuery : IRequest<RentalViewDto?>
    {
        public string Identifier { get; init; } = default!;
    }
}
