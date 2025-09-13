using MediatR;
using MotorDiniz.Domain.Enums;

namespace MotorDiniz.Application.CQRS.DeliveryRiders.Commands
{
    public sealed class CreateDeliveryRiderCommand : IRequest<Unit>
    {
        public string Identifier { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string Cnpj { get; init; } = default!;
        public DateTime BirthDate { get; init; }
        public string CnhNumber { get; init; } = default!;
        public CnhType CnhType { get; init; }
        public string? CnhImageBase64 { get; init; }
    }
}
