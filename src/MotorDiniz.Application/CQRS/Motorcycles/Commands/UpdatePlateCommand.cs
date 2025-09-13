using MediatR;

namespace MotorDiniz.Application.CQRS.Motorcycles.Commands
{
    public sealed class UpdatePlateCommand : IRequest<Unit>
    {
        public string Identifier { get; init; } = default!;
        public string NewPlate { get; init; } = default!;
    }
}
