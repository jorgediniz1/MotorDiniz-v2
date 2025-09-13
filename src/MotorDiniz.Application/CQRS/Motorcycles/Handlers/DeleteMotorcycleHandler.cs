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
    public sealed class DeleteMotorcycleHandler : IRequestHandler<DeleteMotorcycleCommand, Unit>
    {

        public readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteMotorcycleHandler> _logger;

        public DeleteMotorcycleHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork,ILogger<DeleteMotorcycleHandler> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleRepository.GetByIdentifierAsync(request.Identifier, cancellationToken);

            if (motorcycle is null)
            {
                _logger.LogWarning("Motorcycle not found.");
                throw new DomainExceptionValidation("Invalid data.");
            }

            if (await _motorcycleRepository.HasRentalsAsync(motorcycle.Id, cancellationToken))
            {
                _logger.LogWarning("Motorcycle has rentals and cannot be deleted.");
                throw new DomainExceptionValidation("Invalid data: motorcycle has rentals.");
            }

            await _motorcycleRepository.RemoveAsync(motorcycle, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Motorcycle deleted with Identifier {Identifier}", motorcycle.Identifier);

            return Unit.Value;
        }
    }
}
