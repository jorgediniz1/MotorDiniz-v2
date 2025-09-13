using MediatR;
using Microsoft.Extensions.Logging;
using MotorDiniz.Application.CQRS.Rentals.Commands;
using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Enums;
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Domain.Validation;


namespace MotorDiniz.Application.CQRS.Rentals.Handlers
{
    public sealed class CreateRentalHandler : IRequestHandler<CreateRentalCommand, Unit>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IDeliveryRiderRepository _deliveryRiderRepository;
        private readonly IMotorcycleRepository _motocycleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateRentalHandler> _logger;

        public CreateRentalHandler(IRentalRepository rentalRepository, IDeliveryRiderRepository deliveryRiderRepository,
            IMotorcycleRepository motocycleRepository, IUnitOfWork unitOfWork, ILogger<CreateRentalHandler> logger)
        {
            _rentalRepository = rentalRepository;
            _deliveryRiderRepository = deliveryRiderRepository;
            _motocycleRepository = motocycleRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Unit> Handle(CreateRentalCommand command, CancellationToken cancellationToken)
        {
            var deliveryRider = await _deliveryRiderRepository.GetByIdentifierAsync(command.DeliveryRiderIdentifier, cancellationToken);

            if (deliveryRider is null)
            {
                _logger.LogWarning("Delivery rider not found.");
                throw new DomainExceptionValidation("Invalid data: delivery rider not found.");
            }

            var motorcycle = await _motocycleRepository.GetByIdentifierAsync(command.MotorcycleIdentifier, cancellationToken);

            if (motorcycle is null)
            {
                _logger.LogWarning("Motorcycle not found.");
                throw new DomainExceptionValidation("Invalid data: motorcycle not found.");
            }

            if (deliveryRider.CnhType != CnhType.A && deliveryRider.CnhType != CnhType.AB)
            {
                _logger.LogWarning("Delivery rider does not have the required CNH type to rent a motorcycle.");
                throw new DomainExceptionValidation("Invalid data: delivery rider does not have the required CNH type to rent a motorcycle.");
            }

            var plan = command.Plan switch
            {
                7 => RentalPlan.Days7,
                15 => RentalPlan.Days15,
                30 => RentalPlan.Days30,
                45 => RentalPlan.Days45,
                50 => RentalPlan.Days50,
                _ =>  throw new DomainExceptionValidation("Invalid data: rental plan is invalid.")
            };


            // Ainda estou analizando se vai dar tempo de fazer uma tabela RentalPlan para gerenciar os valores das diárias dinamicamente kk.
            var dailyPrice = plan switch
            {
                RentalPlan.Days7 => 30m,
                RentalPlan.Days15 => 28m,
                RentalPlan.Days30 => 22m,
                RentalPlan.Days45 => 20m,
                RentalPlan.Days50 => 18m,
                _ => throw new DomainExceptionValidation("Invalid data: rental plan is invalid.")
            };

            var rentalIdentifier = $"RENTAL-{Guid.NewGuid()}";

            var rental = new Rental(
                        rentalIdentifier,
                        deliveryRider.Id,
                        motorcycle.Id,
                        command.StartDate,
                        command.EndDate,
                        command.ExpectedEndDate,
                        plan,
                        dailyPrice
                    );


            await _rentalRepository.AddAsync(rental, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Rental created with Identifier {Identifier}", rental.Identifier);

            return Unit.Value;


        }
    }
}
