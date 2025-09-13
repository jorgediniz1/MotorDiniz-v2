using MediatR;
using MotorDiniz.Application.CQRS.Rentals.Queries;
using MotorDiniz.Application.DTOs.Rental;
using MotorDiniz.Application.Extensions;
using MotorDiniz.Domain.Interfaces.Repository;

namespace MotorDiniz.Application.CQRS.Rentals.Handlers
{
    public sealed class GetRentalByIdHandler : IRequestHandler<GetRentalByIdQuery, RentalViewDto?>
    {
        private readonly IRentalRepository _rentalRepository;

        public GetRentalByIdHandler(IRentalRepository rentalRepository) => _rentalRepository = rentalRepository;

        public async Task<RentalViewDto?> Handle(GetRentalByIdQuery q, CancellationToken ct)
        {
            var rental = await _rentalRepository.GetByIdentifierWithIncludesAsync(q.Identifier, ct);
            if (rental is null) return null;

            return rental.ToViewDto();

        }
    }
}
