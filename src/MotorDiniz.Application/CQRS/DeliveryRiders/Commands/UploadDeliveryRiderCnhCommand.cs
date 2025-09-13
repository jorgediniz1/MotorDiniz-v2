using MediatR;

namespace MotorDiniz.Application.CQRS.DeliveryRiders.Commands
{
    public sealed class UploadDeliveryRiderCnhCommand : IRequest<Unit>
    {
        public string Identifier { get; init; } = default!;
        public string CnhImageBase64 { get; init; } = default!;
    }
}
