using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Enums;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Domain.Tests.Entities
{
    public class DeliveryRiderTests
    {
        [Fact(DisplayName = "Create delivery rider with valid parameters")]
        public void Create_WithValidParameters_ShouldSucceed()
        {
            var rider = new DeliveryRider(
                identifier: "entregador123",
                name: "Jorge Diniz",
                cnpj: "12.345.678/0001-34",
                birthDate: DateTime.UtcNow.AddYears(-25),
                cnhNumber: "9988776655",
                cnhType: CnhType.AB
            );

            Assert.Equal("entregador123", rider.Identifier);
            Assert.Equal("Jorge Diniz", rider.Name);
            Assert.Equal("12345678000134", rider.Cnpj);
            Assert.Equal("9988776655", rider.CnhNumber);
            Assert.Equal(CnhType.AB, rider.CnhType);
        }

        [Fact(DisplayName = "Create delivery rider with short name throws")]
        public void Create_WithShortName_ShouldThrow()
        {
            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                new DeliveryRider("entregador123", "Jo", "12345678000134", DateTime.UtcNow.AddYears(-20), "12345", CnhType.A)
            );
            Assert.Equal("Name must have at least 3 characters.", ex.Message);
        }

        [Fact(DisplayName = "Create delivery rider with invalid CNPJ length throws")]
        public void Create_WithInvalidCnpj_ShouldThrow()
        {
            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                new DeliveryRider("entregador123", "Jorge Diniz", "123", DateTime.UtcNow.AddYears(-20), "12345", CnhType.A)
            );
            Assert.Equal("CNPJ must have 14 digits.", ex.Message);
        }

        [Fact(DisplayName = "Create delivery rider with empty CNH number throws")]
        public void Create_WithEmptyCnh_ShouldThrow()
        {
            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                new DeliveryRider("entregador123", "Jorge Diniz", "12345678000134", DateTime.UtcNow.AddYears(-20), "", CnhType.A)
            );
            Assert.Equal("CNH number is required.", ex.Message);
        }

        [Fact(DisplayName = "Create delivery rider with invalid CNH type throws")]
        public void Create_WithInvalidCnhType_ShouldThrow()
        {
            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                new DeliveryRider("entregador123", "Jorge Diniz", "12345678000134", DateTime.UtcNow.AddYears(-20), "12345", (CnhType)0)
            );
            Assert.Equal("CNH type is invalid.", ex.Message);
        }

        [Fact(DisplayName = "Update CNH image with empty path throws")]
        public void UpdateCnhImage_Empty_ShouldThrow()
        {
            var rider = new DeliveryRider("entregador123", "Jorge Diniz", "12345678000134", DateTime.UtcNow.AddYears(-20), "12345", CnhType.A);
            var ex = Assert.Throws<DomainExceptionValidation>(() => rider.UpdateCnhImage(""));
            Assert.Equal("CNH image path is required.", ex.Message);
        }

        [Fact(DisplayName = "Update CNH image with too long path throws")]
        public void UpdateCnhImage_TooLong_ShouldThrow()
        {
            var rider = new DeliveryRider("entregador123", "Jorge Diniz", "12345678000134", DateTime.UtcNow.AddYears(-20), "12345", CnhType.A);
            var longPath = new string('a', 501);
            var ex = Assert.Throws<DomainExceptionValidation>(() => rider.UpdateCnhImage(longPath));
            Assert.Equal("CNH image path too long.", ex.Message);
        }

        [Fact(DisplayName = "Update CNH image should set path and touch UpdatedAtUtc")]
        public void UpdateCnhImage_ShouldSetPath_AndTouchUpdated()
        {
            var rider = new DeliveryRider("entregador123", "Jorge Diniz", "12345678000134", DateTime.UtcNow.AddYears(-20), "12345", CnhType.A);
            var before = rider.UpdatedAt;
            rider.UpdateCnhImage("/storage/cnh/entregador123.png");

            Assert.Equal("/storage/cnh/entregador123.png", rider.CnhImagePath);
            Assert.NotEqual(before, rider.UpdatedAt);
        }
    }
}
