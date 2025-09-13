using MotorDiniz.Application.CQRS.Rentals.Commands;
using MotorDiniz.Application.DTOs.Rental;
using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Enums;

namespace MotorDiniz.Application.Extensions
{
    public static class RentalExtensions
    {
        public static RentalViewDto? ToViewDto(this Rental? rental)
        {
            if (rental is null) return null;

            return new RentalViewDto
            {
                Identifier = rental.Identifier,
                DailyPrice = rental.DailyPrice,
                DeliveryRiderIdentifier = rental.DeliveryRider?.Identifier ?? string.Empty,
                MotorcycleIdentifier = rental.Motorcycle?.Identifier ?? string.Empty,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                ExpectedEndDate = rental.ExpectedEndDate,
                ReturnDate = rental.ReturnDate
            };
        }

        public static IEnumerable<RentalViewDto> ToDetailsDto(this IEnumerable<Rental>? list)
        {
            if (list is null)
            {
                return Enumerable.Empty<RentalViewDto>();
            }

            return list.Select(r => r.ToViewDto())
                       .Where(dto => dto is not null)!;
        }

        public static Rental ToEntity(this RentalCreateDto dto, string newIdentifier,
                                      int riderId, int motoId, decimal dailyPrice, RentalPlan plan)
        {
            return new Rental(
                identifier: newIdentifier,
                deliveryRiderId: riderId,
                motorcycleId: motoId,
                startDate: dto.StartDate,
                endDate: dto.EndDate,
                expectedEndDate: dto.ExpectedEndDate,
                plan: plan,
                dailyPrice: dailyPrice
            );
        }

        public static CreateRentalCommand ToCreateCommand(this RentalCreateDto dto)
        {
            if (dto is null) return null;

            return new CreateRentalCommand
            {
                DeliveryRiderIdentifier = dto.DeliveryRiderIdentifier,
                MotorcycleIdentifier = dto.MotorcycleIdentifier,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ExpectedEndDate = dto.ExpectedEndDate,
                Plan = dto.Plan
            };
        }
    }
}
