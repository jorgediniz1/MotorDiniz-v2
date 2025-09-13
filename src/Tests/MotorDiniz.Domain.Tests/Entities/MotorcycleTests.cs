using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Validation;

namespace MotorDiniz.Domain.Tests.Entities
{
    public class MotorcycleTests
    {
        [Fact(DisplayName = "Create motorcycle with valid parameters")]
        public void Create_WithValidParameters_ShouldSucceed()
        {
            var motorcycle = new Motorcycle("moto123", 2024, "Sport 200", "abc-1234");
            Assert.Equal("moto123", motorcycle.Identifier);
            Assert.Equal(2024, motorcycle.Year);
            Assert.Equal("Sport 200", motorcycle.Model);
            Assert.Equal("ABC-1234", motorcycle.Plate);
        }

        [Fact(DisplayName = "Create motorcycle with empty plate throws")]
        public void Create_WithEmptyPlate_ShouldThrow()
        {
            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                new Motorcycle("moto123", 2024, "Modelo X", "")
            );
            Assert.Equal("Plate is required.", ex.Message);
        }

        [Fact(DisplayName = "Change plate with valid value should update and touch updated")]
        public void ChangePlate_Valid_ShouldUpdateAndTouchUpdated()
        {
            var motorcycle = new Motorcycle("moto123", 2024, "Modelo X", "AAA-0001");
            var before = motorcycle.UpdatedAt;

            motorcycle.ChangePlate("cdx-0101");

            Assert.Equal("CDX-0101", motorcycle.Plate);
            Assert.NotEqual(before, motorcycle.UpdatedAt);
        }

        [Fact(DisplayName = "Change plate with empty value throws")]
        public void ChangePlate_Empty_ShouldThrow()
        {
            var motorcycle = new Motorcycle("moto123", 2024, "Modelo X", "AAA-0001");
            var ex = Assert.Throws<DomainExceptionValidation>(() => motorcycle.ChangePlate(""));
            Assert.Equal("Plate is required.", ex.Message);
        }
    }
}
