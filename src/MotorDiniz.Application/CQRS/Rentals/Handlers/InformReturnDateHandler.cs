using MediatR;
using Microsoft.Extensions.Logging;
using MotorDiniz.Application.CQRS.Rentals.Commands;
using MotorDiniz.Application.Interfaces;
using MotorDiniz.Domain.Interfaces.Repository;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Application.CQRS.Rentals.Handlers
{
    public sealed class InformReturnDateHandler : IRequestHandler<InformReturnDateCommand, Unit>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRentalPricingService _pricingService;
        private readonly ILogger<InformReturnDateHandler> _logger;

        public InformReturnDateHandler(IRentalRepository rentalRepository, IUnitOfWork unitOfWork, 
            ILogger<InformReturnDateHandler> logger, IRentalPricingService pricingService)
        {
            _rentalRepository = rentalRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _pricingService = pricingService;
        }

        public async Task<Unit> Handle(InformReturnDateCommand c, CancellationToken ct)
        {
            var rental = await _rentalRepository.GetByIdentifierAsync(c.Identifier, ct);

            if (rental is null)
            {
                _logger.LogWarning("Rental not found.");
                throw new DomainExceptionValidation("Invalid data: rental not found.");
            }

            var planDays = (int)rental.Plan;

            var total = _pricingService.CalculateTotal(
                planDays,
                rental.DailyPrice,
                rental.StartDate,
                rental.ExpectedEndDate,
                c.ReturnDate);

            _logger.LogInformation("Calculated total amount {Total} for rental Identifier {Identifier}", total, rental.Identifier);

            rental.InformReturn(c.ReturnDate, total);

            await _unitOfWork.SaveChangesAsync(ct);
            _logger.LogInformation("Return date informed for rental Identifier {Identifier}", rental.Identifier);

            return Unit.Value;
        }
    }
}
