using MediatR;

namespace MotorDiniz.Application.CQRS.Rentals.Commands
{
    public sealed record CreateRentalCommand : IRequest<Unit>
    {
        public string DeliveryRiderIdentifier { get; init; } = default!;
        public string MotorcycleIdentifier { get; init; } = default!;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public DateTime ExpectedEndDate { get; init; }
        public int Plan { get; init; }
    }
}
