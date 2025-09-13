using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MotorDiniz.Application.CQRS.Motorcycles.Commands;
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Application.CQRS.Motorcycles.Handlers
{
    public sealed class UpdatePlateHandler : IRequestHandler<UpdatePlateCommand, Unit>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdatePlateHandler> _logger;

        public UpdatePlateHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork, ILogger<UpdatePlateHandler> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdatePlateCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleRepository.GetByIdentifierAsync(request.Identifier, cancellationToken);
            if (motorcycle is null)
            {
                _logger.LogWarning("Motorcycle not found.");
                throw new DomainExceptionValidation("Invalid data.");
            }

            if (await _motorcycleRepository.PlateExistsAsync(request.NewPlate, cancellationToken))
            {
                _logger.LogWarning("Plate already exists.");
                throw new DomainExceptionValidation("Invalid data: plate already exists.");
            }

            motorcycle.ChangePlate(request.NewPlate);
            await _motorcycleRepository.UpdateAsync(motorcycle, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Motorcycle plate updated for Identifier {Identifier}", motorcycle.Identifier);

            return Unit.Value;
        }


    }
}
