using MotorDiniz.Domain.Enums;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Domain.Entities
{
    public sealed class DeliveryRider : EntityBase
    {
        public string Identifier { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public string Cnpj { get; private set; } = default!;
        public DateTime BirthDate { get; private set; }
        public string CnhNumber { get; private set; } = default!;
        public CnhType CnhType { get; private set; }
        public string? CnhImagePath { get; private set; }

        // EF
        private DeliveryRider() { }

        public DeliveryRider(string identifier, string name, string cnpj, DateTime birthDate,
                       string cnhNumber, CnhType cnhType)
        {
            Validate(identifier, name, cnpj, birthDate, cnhNumber, cnhType);

            Identifier = identifier.Trim();
            Name = name.Trim();
            Cnpj = OnlyDigits(cnpj);
            BirthDate = birthDate;
            CnhNumber = cnhNumber.Trim();
            CnhType = cnhType;
        }

        public void UpdateCnhImage(string storagePathOrUrl)
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(storagePathOrUrl), "CNH image path is required.");
            DomainExceptionValidation.When(storagePathOrUrl.Length > 500, "CNH image path too long.");
            CnhImagePath = storagePathOrUrl;
            TouchUpdated();
        }

        private static void Validate(string identifier, string name, string cnpj,
                                     DateTime birthDate, string cnhNumber, CnhType cnhType)
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(identifier), "Identifier is required.");
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(name), "Name is required.");
            DomainExceptionValidation.When(name.Length < 3, "Name must have at least 3 characters.");
            DomainExceptionValidation.When(name.Length > 120, "Name must have at most 120 characters.");
            var cnpjDigits = OnlyDigits(cnpj);
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(cnpjDigits), "CNPJ is required.");
            DomainExceptionValidation.When(cnpjDigits.Length != 14, "CNPJ must have 14 digits.");
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(cnhNumber), "CNH number is required.");
            DomainExceptionValidation.When(cnhNumber.Length < 5, "CNH number must have at least 5 characters.");
            DomainExceptionValidation.When((int)cnhType <= 0, "CNH type is invalid.");
            DomainExceptionValidation.When(birthDate > DateTime.UtcNow.AddDays(1), "Birth date is invalid.");
        }

        private static string OnlyDigits(string input) => new string(input?.Where(char.IsDigit).ToArray() ?? []);
    }
}
