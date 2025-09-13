using MotorDiniz.Domain.Enums;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Domain.Entities
{
    public sealed class Rental : EntityBase
    {
        public string Identifier { get; private set; } = default!;
        public int DeliveryRiderId { get; private set; }
        public int MotorcycleId{ get; private set; }

        public DeliveryRider? DeliveryRider { get; private set; }
        public Motorcycle? Motorcycle { get; private set; }


        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime ExpectedEndDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }

        public RentalPlan Plan { get; private set; }
        public decimal DailyPrice { get; private set; }

        public decimal? TotalAmount { get; private set; }


        // EF
        private Rental() { }

        public Rental(string identifier, int deliveryRiderId, int motorcycleId,
                      DateTime startDate, DateTime endDate, DateTime expectedEndDate,
                      RentalPlan plan, decimal dailyPrice)
        {
            Validate(identifier, deliveryRiderId, motorcycleId,
                     startDate, endDate, expectedEndDate, plan, dailyPrice);

            Identifier = identifier;
            DeliveryRiderId = deliveryRiderId;
            MotorcycleId = motorcycleId;
            StartDate = startDate;
            EndDate = endDate;
            ExpectedEndDate = expectedEndDate;
            Plan = plan;
            DailyPrice = dailyPrice;
        }

        public void InformReturn(DateTime returnDate, decimal total)
        {
            DomainExceptionValidation.When(returnDate < StartDate, "Return date cannot be before start date.");
            ReturnDate = returnDate;
            TotalAmount = Math.Round(total, 2, MidpointRounding.AwayFromZero);
            TouchUpdated();
        }

        private static void Validate(string identifier, int deliveryRiderId, int motoId,
                                     DateTime startDate, DateTime endDate, DateTime expectedEndDate,
                                     RentalPlan plan, decimal dailyPrice)
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(identifier), "Identifier is required.");
            DomainExceptionValidation.When(deliveryRiderId <= 0, "DeliveryRiderId is invalid.");
            DomainExceptionValidation.When(motoId <= 0, "MotorcycleId is invalid.");
            DomainExceptionValidation.When(dailyPrice <= 0, "Daily price must be greater than zero.");
            DomainExceptionValidation.When((int)plan != 7 && (int)plan != 15 && (int)plan != 30 && (int)plan != 45 && (int)plan != 50,
                "Rental plan is invalid.");

            DomainExceptionValidation.When(endDate < startDate, "End date cannot be before start date.");
            DomainExceptionValidation.When(expectedEndDate < endDate, "Expected end date cannot be before start date.");
        }
    }
}
