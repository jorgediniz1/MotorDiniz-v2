using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Domain.Entities
{
    public sealed class Motorcycle : EntityBase
    {
        public string Identifier { get; private set; } = default!;
        public int Year { get; private set; }
        public string Model { get; private set; } = default!;
        public string Plate { get; private set; } = default!;

        // EF Core
        private Motorcycle() { }

        public Motorcycle(string identifier, int year, string model, string plate)
        {
            Validate(identifier, year, model, plate);
            Identifier = identifier.Trim();
            Year = year;
            Model = model.Trim();
            Plate = plate.Trim().ToUpperInvariant();
        }

        public void ChangePlate(string newPlate)
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(newPlate), "Plate is required.");
            DomainExceptionValidation.When(newPlate.Length < 3, "Plate must have at least 3 characters.");
            DomainExceptionValidation.When(newPlate.Length > 12, "Plate must have at most 12 characters.");

            Plate = newPlate.Trim().ToUpperInvariant();
            TouchUpdated();
        }

        private static void Validate(string identifier, int year, string model, string plate)
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(identifier), "Identifier is required.");
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(model), "Model is required.");
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(plate), "Plate is required.");
            DomainExceptionValidation.When(model.Length < 2, "Model must have at least 2 characters.");
            DomainExceptionValidation.When(model.Length > 120, "Model must have at most 120 characters.");
            DomainExceptionValidation.When(plate.Length < 3, "Plate must have at least 3 characters.");
            DomainExceptionValidation.When(plate.Length > 12, "Plate must have at most 12 characters.");
        }
    }
}
