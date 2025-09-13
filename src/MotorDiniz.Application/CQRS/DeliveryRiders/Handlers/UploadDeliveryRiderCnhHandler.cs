using MediatR;
using Microsoft.Extensions.Logging;
using MotorDiniz.Application.CQRS.DeliveryRiders.Commands;
using MotorDiniz.Application.Interfaces; // IObjectStorage
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Application.CQRS.DeliveryRiders.Handlers
{
    public sealed class UploadDeliveryRiderCnhHandler : IRequestHandler<UploadDeliveryRiderCnhCommand, Unit>
    {
        private readonly IDeliveryRiderRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IObjectStorage _storage;
        private readonly ILogger<UploadDeliveryRiderCnhHandler> _logger;

        public UploadDeliveryRiderCnhHandler(
            IDeliveryRiderRepository repo,
            IUnitOfWork uow,
            IObjectStorage storage,
            ILogger<UploadDeliveryRiderCnhHandler> logger)
        {
            _repo = repo;
            _uow = uow;
            _storage = storage;
            _logger = logger;
        }

        public async Task<Unit> Handle(UploadDeliveryRiderCnhCommand command, CancellationToken ct)
        {
            var rider = await _repo.GetByIdentifierAsync(command.Identifier, ct);
            if (rider is null)
            {
                _logger.LogWarning("Delivery rider not found.");
                throw new DomainExceptionValidation("Invalid data: delivery rider not found.");
            }

            var (bytes, contentType) = DecodeAndDetect(command.CnhImageBase64);
            var objectKey = await _storage.UploadCnhImageAsync(rider.Identifier, bytes, contentType, ct);
            
            _logger.LogInformation("CNH image uploaded for rider Identifier {Identifier}", rider.Identifier);

            rider.UpdateCnhImage(objectKey);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Delivery rider CNH image updated for Identifier {Identifier}", rider.Identifier);

            return Unit.Value;
        }

        private static (byte[] bytes, string contentType) DecodeAndDetect(string base64)
        {
            try
            {
                var data = Convert.FromBase64String(base64);
                // PNG: 89 50 4E 47
                if (data.Length >= 8 && data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47)
                    return (data, "image/png");
                // BMP: 42 4D
                if (data.Length >= 2 && data[0] == 0x42 && data[1] == 0x4D)
                    return (data, "image/bmp");

                throw new DomainExceptionValidation("CNH image must be PNG or BMP.");
            }
            catch
            {
                throw new DomainExceptionValidation("CNH image must be PNG or BMP.");
            }
        }
    }
}
