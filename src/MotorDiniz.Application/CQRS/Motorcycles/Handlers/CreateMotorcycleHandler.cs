using MediatR;
using Microsoft.Extensions.Logging;
using MotorDiniz.Application.CQRS.Motorcycles.Commands;
using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Domain.Validation;
using MotorDiniz.Producer.Events;
using MotorDiniz.Producer.Interface;

namespace MotorDiniz.Application.CQRS.Motorcycles.Handlers
{
    public sealed class CreateMotorcycleHandler : IRequestHandler<CreateMotorcycleCommand, Unit>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMotorcycleProducer _motorcycleProducer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateMotorcycleHandler> _logger;

        public CreateMotorcycleHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork,
            IMotorcycleProducer motorcycleProducer,ILogger<CreateMotorcycleHandler> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _motorcycleProducer = motorcycleProducer;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Unit> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var plateExists = await _motorcycleRepository.PlateExistsAsync(request.Plate, cancellationToken);

            if (plateExists)
            {
                _logger.LogWarning("Plate already exists.");
                throw new DomainExceptionValidation("Invalid data: plate already exists.");
            }

            var motorcycle = new Motorcycle(request.Identifier, request.Year, request.Model, request.Plate);

            await _motorcycleRepository.AddAsync(motorcycle, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Motorcycle created with Identifier {Identifier}", motorcycle.Identifier);

            var evento = new MotorcycleCreatedEvent(motorcycle.Identifier, motorcycle.Year, motorcycle.Model, motorcycle.Plate);

            await _motorcycleProducer.PublishCreatedAsync(evento, cancellationToken);

            _logger.LogInformation("MotorcycleCreatedEvent published for Identifier {Identifier}", motorcycle.Identifier);

            return Unit.Value;
        }
    }
}
