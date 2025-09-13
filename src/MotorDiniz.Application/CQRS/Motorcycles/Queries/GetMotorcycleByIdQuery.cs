using MediatR;
using MotorDiniz.Application.DTOs.Motorcycle;

namespace MotorDiniz.Application.CQRS.Motorcycles.Queries
{
    public sealed class GetMotorcycleByIdQuery : IRequest<MotorcycleViewDto?>
    {
        public string Identifier { get; init; } = default!;
    }
}
