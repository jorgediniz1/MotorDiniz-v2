using MediatR;

namespace MotorDiniz.Application.CQRS.Motorcycles.Commands
{
    public sealed class CreateMotorcycleCommand : IRequest<Unit>
    {
        public string Identifier { get; init; } = default!;
        public int Year { get; init; }
        public string Model { get; init; } = default!;
        public string Plate { get; init; } = default!;
    }
}
