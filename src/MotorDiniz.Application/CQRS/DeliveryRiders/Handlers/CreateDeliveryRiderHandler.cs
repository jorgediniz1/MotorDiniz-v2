using MediatR;
using Microsoft.Extensions.Logging;
using MotorDiniz.Application.CQRS.DeliveryRiders.Commands;
using MotorDiniz.Application.Interfaces; // IObjectStorage
using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Enums;
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Application.CQRS.DeliveryRiders.Handlers
{
    public sealed class CreateDeliveryRiderHandler : IRequestHandler<CreateDeliveryRiderCommand, Unit>
    {
        private readonly IDeliveryRiderRepository _deliveryRiderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IObjectStorage _storage;
        private readonly ILogger<CreateDeliveryRiderHandler> _logger;

        public CreateDeliveryRiderHandler(
            IDeliveryRiderRepository deliveryRiderRepository,
            IUnitOfWork unitOfWork,
            IObjectStorage storage,
            ILogger<CreateDeliveryRiderHandler> logger)
        {
            _deliveryRiderRepository = deliveryRiderRepository;
            _unitOfWork = unitOfWork;
            _storage = storage;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateDeliveryRiderCommand command, CancellationToken ct)
        {
            if (await _deliveryRiderRepository.CnpjExistsAsync(command.Cnpj, ct)) 
            {
                _logger.LogWarning("CNPJ already exists.");
                throw new DomainExceptionValidation("Invalid data: CNPJ already exists.");
            }

            if (await _deliveryRiderRepository.CnhExistsAsync(command.CnhNumber, ct))
            {
                _logger.LogWarning("CNH number already exists.");
                throw new DomainExceptionValidation("Invalid data: CNH number already exists.");
            }

            DomainExceptionValidation.When(!Enum.IsDefined(typeof(CnhType), command.CnhType), "Invalid CNH type.");

            var rider = new DeliveryRider(
                command.Identifier,
                command.Name,
                command.Cnpj,
                command.BirthDate,
                command.CnhNumber,
                command.CnhType
            );

            if (!string.IsNullOrWhiteSpace(command.CnhImageBase64))
            {
                var (bytes, contentType) = DecodeAndDetect(command.CnhImageBase64);
                var objectKey = await _storage.UploadCnhImageAsync(rider.Identifier, bytes, contentType, ct);
                rider.UpdateCnhImage(objectKey);
            }

            await _deliveryRiderRepository.AddAsync(rider, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Delivery rider created with Identifier {Identifier}", rider.Identifier);

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
