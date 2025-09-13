using MediatR;

namespace MotorDiniz.Application.CQRS.Motorcycles.Commands
{
    public sealed class DeleteMotorcycleCommand : IRequest<Unit>
    {
        public string Identifier { get; init; } = default!;

    }
}
