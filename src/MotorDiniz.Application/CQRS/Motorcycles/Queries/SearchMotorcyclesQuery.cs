using MotorDiniz.Application.DTOs.Motorcycle;
using MediatR;

namespace MotorDiniz.Application.CQRS.Motorcycles.Queries
{
    public sealed class SearchMotorcyclesQuery : IRequest<IReadOnlyList<MotorcycleViewDto>>
    {
        public string? Plate { get; init; }
    }
}
